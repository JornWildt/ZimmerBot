using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Processors;

namespace Holidays.ZimmerBot.AddOn
{
  public class HolidaysApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in scripts
      ProcessorRegistry.RegisterProcessor("Holidays.DateOfHoliday", HolidaysProcessor.DateOfHoliday);
    }


    public void Shutdown()
    {
    }
  }
}
