using System.Collections.Generic;
using log4net;
using ZimmerBot.Core.Processors;

namespace Jitsi.ZimmerBot.AddOn
{
  public static class JitsiProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(JitsiProcessor));


    public static ProcessorOutput Meeting(ProcessorInput input)
    {
      Logger.Debug($"Get meeting URL");

      string meetingId = CBrain.Toolbox.StringUtility.GetRandomStringWithLettersAndDigitsOnly(10);

      Dictionary<string, object> result = new Dictionary<string, object>
      {
        ["url"] = "https://meet.cbrain.net/" + meetingId
      };

      return new ProcessorOutput(result);
    }
  }
}
