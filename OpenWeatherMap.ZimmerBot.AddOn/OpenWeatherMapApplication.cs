using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Processors;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public class OpenWeatherMapApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in scripts
      ProcessorRegistry.RegisterProcessor("OWM.Forecast", OpenWeatherMapProcessor.Forecast);
      ProcessorRegistry.RegisterProcessor("OWM.Weather", OpenWeatherMapProcessor.Weather);
    }


    public void Shutdown()
    {
    }
  }
}
