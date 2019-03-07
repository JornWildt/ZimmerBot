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


    public static ProcessorOutput Forecast(ProcessorInput input)
    {
      string location = input.GetParameter<string>(0);
      Logger.Debug($"Lookup weather forecast for '{location}' at Open Weather Map.");

      try
      {
        if (!Cache.ContainsKey(location) || Cache[location].Timestamp < DateTime.Now.AddHours(-1))
        {
          Dictionary<string, object> parameters = GetForecast(location);

          Cache[location] = new CacheEntry
          {
            Timestamp = DateTime.Now,
            Output = new ProcessorOutput(parameters)
          };
        }

        return Cache[location].Output;
      }
      catch (Exception ex)
      {
        Logger.Debug(ex);
        return new ProcessorOutput("error", null);
      }
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


    private static Dictionary<string,string> ForecastRestOfDay(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast, 0);

      Dictionary<string, string> data = CalcForecastData(forecast, 0, firstTomorrowIndex);
      data["period"] = "Resten af dagen";

      return data;
    }


    private static Dictionary<string, string> ForecastTomorrow(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast, 6);

      Dictionary<string, string> data = CalcForecastData(forecast, firstTomorrowIndex, forecast.List.Count);
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


    private static Dictionary<string, string> CalcForecastData(OpenWeatherMapAPI.Forecast forecast, int start, int end)
    {
      decimal minTemp = 100;
      decimal maxTemp = -100;
      Dictionary<string, int> weatherTypeCount = new Dictionary<string, int>();
      Dictionary<string, int> windDirCodeCount = new Dictionary<string, int>();

      for (int i = start; i<end; ++i)
      {
        var f = forecast.List[i];
        if (f.Temperature.Min < minTemp)
          minTemp = f.Temperature.Min;
        if (f.Temperature.Max > maxTemp)
          maxTemp = f.Temperature.Max;

        if (!weatherTypeCount.ContainsKey(f.Symbol.Name))
          weatherTypeCount[f.Symbol.Name] = 0;
        weatherTypeCount[f.Symbol.Name]++;

        if (!windDirCodeCount.ContainsKey(f.WindDirection.Code))
          windDirCodeCount[f.WindDirection.Code] = 0;
        windDirCodeCount[f.WindDirection.Code]++;
      }

      var mostUsedWeatherType = weatherTypeCount.OrderBy(i => i.Value > i.Value).FirstOrDefault();
      var mostUsedWindDirCode = windDirCodeCount.OrderBy(i => i.Value > i.Value).FirstOrDefault();

      return new Dictionary<string, string>
      {
        ["minTemp"] = Math.Round(minTemp).ToString(),
        ["maxTemp"] = Math.Round(maxTemp).ToString(),
        ["weather"] = mostUsedWeatherType.Key,
        ["windDir"] = mostUsedWindDirCode.Key
      };
    }
  }
}
