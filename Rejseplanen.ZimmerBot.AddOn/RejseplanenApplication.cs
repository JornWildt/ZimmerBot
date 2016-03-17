using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Knowledge;

namespace Rejseplanen.ZimmerBot.AddOn
{
  public class RejseplanenApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      ProcessorRegistry.AddProcessor("Rejseplanen.FindStation", RejseplanenProcessor.FindStation);
    }


    public void Shutdown()
    {
    }
  }
}
