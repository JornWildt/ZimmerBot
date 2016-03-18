using ZimmerBot.Core.AddOnHandling;
using ZimmerBot.Core.Knowledge;


namespace Rejseplanen.ZimmerBot.AddOn
{
  /// <summary>
  /// This class is instantiated automatically by ZimmerBot on startup and shutdown and handles the corresponding operations.
  /// </summary>
  public class RejseplanenApplication : IZimmerBotAddOn
  {
    public void Initialize()
    {
      // Register processor functions available for use in rules
      ProcessorRegistry.AddProcessor("Rejseplanen.FindStoppested", RejseplanenProcessor.FindStop);
    }


    public void Shutdown()
    {
    }
  }
}
