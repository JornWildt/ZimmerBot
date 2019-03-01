using System.Collections.Generic;
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
      OpenWeatherMapAPI.Forecast result = api.GetForecast(location);

      parameters["result"] = $"Temp: {result.list[0].main.temp}";

      return new ProcessorOutput(parameters);
    }

  }
}
