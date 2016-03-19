using System;
using System.Collections.Generic;
using log4net;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Utilities;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(RejseplanenProcessor));

    public class FindStopResponse
    {
      public string stopName { get; set; }
    }

    public static string FindStop(ProcessorInput input)
    {
      string name = input.GetParameter<string>(0);

      var parameters = new Dictionary<string, object>();
      foreach (var item in input.Context.Match.Matches)
        parameters[item.Key] = item.Value;

      Logger.Debug($"Looking for station '{name}' in Rejseplanen");

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
            parameters["stopName"] = stop.name;
            return TextMerge.MergeTemplate(input.OutputTemplates, parameters);
          }
        }

        return TextMerge.MergeTemplate(input.OutputTemplates, "empty", parameters);
      }
      catch (Exception ex)
      {
        Logger.Error("Failed to access Rejseplanen", ex);
        return TextMerge.MergeTemplate(input.OutputTemplates, "error", parameters);
      }
    }
  }
}

