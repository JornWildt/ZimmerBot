using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ZimmerBot.Core.Processors;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public static class OpenWeatherMapProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(OpenWeatherMapProcessor));


    public static ProcessorOutput Forecast(ProcessorInput input)
    {
      string location = input.GetParameter<string>(0);
      Logger.Debug($"Lookup weather forecast for '{location}' at Open Weather Map.");

      var parameters = new Dictionary<string, object>();

      OpenWeatherMapAPI api = new OpenWeatherMapAPI();
      OpenWeatherMapAPI.Forecast result = api.GetForecast(location, 48);

      var restOfDay = ForecastRestOfDay(result);
      var tomorrow = ForecastTomorrow(result);

      parameters["today"] = restOfDay;
      parameters["tomorrow"] = tomorrow;

      return new ProcessorOutput(parameters);
    }


    private static Dictionary<string,string> ForecastRestOfDay(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast);

      Dictionary<string, string> data = CalcForecastData(forecast, 0, firstTomorrowIndex);
      data["period"] = "Resten af dagen";

      return data;
    }


    private static Dictionary<string, string> ForecastTomorrow(OpenWeatherMapAPI.Forecast forecast)
    {
      int firstTomorrowIndex = FindFirstTomorrow(forecast);

      Dictionary<string, string> data = CalcForecastData(forecast, firstTomorrowIndex, forecast.List.Count);
      data["period"] = "I morgen";

      return data;
    }


    private static int FindFirstTomorrow(OpenWeatherMapAPI.Forecast forecast)
    {
      int today = DateTime.Now.DayOfYear;
      int firstTomorrowIndex = 0;

      for (int i = 0; i < forecast.List.Count; ++i)
      {
        if (forecast.List[i].From.DayOfYear != today)
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
      }

      var mostUsedWeatherType = weatherTypeCount.OrderBy(i => i.Value > i.Value).FirstOrDefault();

      return new Dictionary<string, string>
      {
        ["minTemp"] = Math.Round(minTemp).ToString(),
        ["maxTemp"] = Math.Round(maxTemp).ToString(),
        ["weather"] = mostUsedWeatherType.Key
      };
    }
  }
}
