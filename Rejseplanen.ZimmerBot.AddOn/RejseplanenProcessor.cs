using System;
using log4net;
using Ramone;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;
using WebRequest = Ramone.Request;
using WebResponse = Ramone.Response;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(RejseplanenProcessor));


    public static Func<string> FindStop(ProcessorInput input)
    {
      string name = input.GetParameter<string>(0);

      try
      {
        RejseplanenAPI api = new RejseplanenAPI();
        LocationList locations = api.GetLocations(name);

        if (locations.Items != null && locations.Items.Length > 0)
        {
          StopLocation stop = locations.Items[0] as StopLocation;
          CoordLocation coord = locations.Items[0] as CoordLocation;

          if (stop != null)
          {
            var parameters = new
            {
              StopName = stop.name,
              StopId = stop.id
            };

            return () => TextMerge.MergeTemplate(input.Template, parameters);
          }
        }

        return () => "EMPTY";
      }
      catch (Exception ex)
      {
        Logger.Error("Failed to access Rejseplanen", ex);
        return () => "ERROR: " + ex.Message;
      }
    }
  }
}

