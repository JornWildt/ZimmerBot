using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using ZimmerBot.Core.Processors;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public static class OpenWeatherMapProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(OpenWeatherMapProcessor));

    class CacheEntry
    {
      public DateTime Timestamp { get; set; }
      public ProcessorOutput Output { get; set; }
    }

    static ConcurrentDictionary<string, CacheEntry> Cache = new ConcurrentDictionary<string, CacheEntry>();


    public static ProcessorOutput Weather(ProcessorInput input)
    {
      string location = input.GetParameter<string>(0);
      Logger.Debug($"Lookup current weather for '{location}' at Open Weather Map.");

      return GetCached(
        CacheKey("weather", location),
        () =>
        {
          Dictionary<string, object> result = GetWeather(location);
          return new ProcessorOutput(result);
        });
    }


    public static ProcessorOutput Forecast(ProcessorInput input)
    {
      string location = input.GetParameter<string>(0);
      Logger.Debug($"Lookup weather forecast for '{location}' at Open Weather Map.");

      return GetCached(
        CacheKey("weather", location),
        () =>
        {
          Dictionary<string, object> result = GetForecast(location);
          return new ProcessorOutput(result);
        });
    }


    private static ProcessorOutput GetCached(string key, Func<ProcessorOutput> action)
    {
      try
      {
        if (!Cache.ContainsKey(key) || Cache[key].Timestamp < DateTime.Now.AddHours(-1))
        {
          ProcessorOutput result = action();

          Cache[key] = new CacheEntry
          {
            Timestamp = DateTime.Now,
            Output = result
          };
        }

        return Cache[key].Output;
      }
      catch (Exception ex)
      {
        Logger.Debug(ex);
        return new ProcessorOutput("error", null);
      }
    }


    private static string CacheKey(string function, string location) => function + ":" + location;


    private static Dictionary<string, object> GetWeather(string location)
    {
      OpenWeatherMapAPI api = new OpenWeatherMapAPI();
      OpenWeatherMapAPI.Current result = api.GetWeather(location);

      var weatherData = new Dictionary<string, object>
      {
        ["temp"] = Math.Round(result.Temperature.Value).ToString(),
        ["weather"] = result.Weather.Value,
        ["windDir"] = result.Wind.Direction.Name,
        ["windSpeed"] = Math.Round(result.Wind.Speed.MetersPerSecond).ToString(),
      };

      bool isRain = result.Precipitation?.Mode?.Contains("rain") ?? false;
      bool isSnow = result.Precipitation?.Mode?.Contains("snow") ?? false;

      CalculateState(weatherData, result.Wind.Speed.MetersPerSecond, result.Clouds.Value, 
        isRain, isSnow, 
        result.City.Sun.Rise, result.City.Sun.Set);

      return weatherData;
    }


    private static Dictionary<string, object> GetForecast(string location)
    {
      var parameters = new Dictionary<string, object>();

      OpenWeatherMapAPI api = new OpenWeatherMapAPI();
      OpenWeatherMapAPI.Forecast result = api.GetForecast(location, 48);

      var restOfDay = ForecastRestOfDay(result);
      var tomorrow = ForecastTomorrow(result);

      parameters["today"] = restOfDay;
      parameters["tomorrow"] = tomorrow;
      return parameters;
    }


    private static Dictionary<string, object> ForecastRestOfDay(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast, 0);

      Dictionary<string, object> data = CalcForecastData(forecast, 0, firstTomorrowIndex);
      data["period"] = "Resten af dagen";

      return data;
    }


    private static Dictionary<string, object> ForecastTomorrow(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast, 6);

      Dictionary<string, object> data = CalcForecastData(forecast, firstTomorrowIndex, forecast.List.Count);
      data["period"] = "I morgen";

      return data;
    }


    private static int FindFirstTomorrow(OpenWeatherMapAPI.Forecast forecast, int timeOfDay)
    {
      int today = DateTime.Now.DayOfYear;
      int firstTomorrowIndex = 0;

      for (int i = 0; i < forecast.List.Count; ++i)
      {
        if (forecast.List[i].From.DayOfYear != today && forecast.List[i].From.Hour >= timeOfDay)
        {
          firstTomorrowIndex = i;
          break;
        }
      }

      return firstTomorrowIndex;
    }


    private static Dictionary<string, object> CalcForecastData(OpenWeatherMapAPI.Forecast forecast, int start, int end)
    {
      decimal minTemp = 100;
      decimal maxTemp = -100;
      decimal minWind = 100;
      decimal maxWind = -100;
      decimal precipation = 0;
      decimal clouds = 0;
      bool isRain = false;
      bool isSnow = false;
      Dictionary<string, int> weatherTypeCount = new Dictionary<string, int>();
      Dictionary<string, int> windDirCodeCount = new Dictionary<string, int>();

      for (int i = start; i<end; ++i)
      {
        var f = forecast.List[i];

        if (f.Temperature.Min < minTemp)
          minTemp = f.Temperature.Min;
        if (f.Temperature.Max > maxTemp)
          maxTemp = f.Temperature.Max;

        if (f.WindSpeed.MetersPerSecond < minWind)
          minWind = f.WindSpeed.MetersPerSecond;
        if (f.WindSpeed.MetersPerSecond > maxWind)
          maxWind = f.WindSpeed.MetersPerSecond;

        if (!weatherTypeCount.ContainsKey(f.Symbol.Name))
          weatherTypeCount[f.Symbol.Name] = 0;
        weatherTypeCount[f.Symbol.Name]++;

        if (!windDirCodeCount.ContainsKey(f.WindDirection.Code))
          windDirCodeCount[f.WindDirection.Code] = 0;
        windDirCodeCount[f.WindDirection.Code]++;

        clouds += f.Clouds.all;

        precipation += f.Precipation?.Value ?? 0m;
        isRain = isRain || (f.Precipation?.type?.Contains("rain") ?? false);
        isSnow = isSnow || (f.Precipation?.type?.Contains("snow") ?? false);
      }

      var averageWind = (minWind + maxWind) / 2.0m;
      var mostUsedWeatherType = weatherTypeCount.OrderBy(i => i.Value > i.Value).FirstOrDefault();
      var mostUsedWindDirCode = windDirCodeCount.OrderBy(i => i.Value > i.Value).FirstOrDefault();

      decimal averageClouds = clouds / (end - start);

      var forecastData = new Dictionary<string, object>
      {
        ["minTemp"] = Math.Round(minTemp).ToString(),
        ["maxTemp"] = Math.Round(maxTemp).ToString(),
        ["weather"] = mostUsedWeatherType.Key,
        ["windDir"] = mostUsedWindDirCode.Key,
        ["windMin"] = Math.Round(minWind).ToString(),
        ["windMax"] = Math.Round(maxWind).ToString(),
        ["windSpeed"] = Math.Round(averageWind).ToString()
      };

      CalculateState(forecastData, maxWind, averageClouds, isRain, isSnow, forecast.Sun.Rise, forecast.Sun.Set);

      return forecastData;
    }


    private static void CalculateState(
      Dictionary<string, object> output, decimal wind, decimal clouds, 
      bool isRain, bool isSnow,
      DateTime sunrise, DateTime sunset)
    {
      bool isStrongBreeze = wind >= 10.8m;
      bool isGale = wind >= 13.9m;
      bool isStorm = wind >= 24.5m;

      bool isCloudy = clouds > 75;
      bool isClear = clouds < 15;

      DateTime now = DateTime.Now;
      bool isSunny = isClear && sunrise < now && now < sunset;

      output["isStrongBreeze"] = isStrongBreeze ? "1" : null;
      output["isGale"] = isGale ? "1" : null;
      output["isStorm"] = isStorm ? "1" : null;
      output["isRain"] = isRain ? "1" : null;
      output["isSnow"] = isSnow ? "1" : null;
      output["isClear"] = isClear ? "1" : null;
      output["isCloudy"] = isCloudy ? "1" : null;
      output["isSunny"] = isSunny ? "1" : null;
    }
  }
}
