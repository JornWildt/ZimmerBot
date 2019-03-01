using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Processors;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public class OpenWeatherMapApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in rules
      ProcessorRegistry
        .RegisterProcessor("OWM.Forecast", OpenWeatherMapProcessor.Forecast);
    }


    public void Shutdown()
    {
    }
  }
}
