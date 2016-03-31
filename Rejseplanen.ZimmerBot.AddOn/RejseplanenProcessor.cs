using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using ZimmerBot.Core.Processors;
using ZimmerBot.Core.Utilities;


namespace Rejseplanen.ZimmerBot.AddOn
{
  /// <summary>
  /// This class exposes various Rejseplanen functions to be used in ZimmerBot.
  /// </summary>
  public static class RejseplanenProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(RejseplanenProcessor));


    public static ProcessorOutput FindStop(ProcessorInput input)
    {
      string name = input.GetParameter<string>(0);

      var parameters = new Dictionary<string, object>();

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
            return new ProcessorOutput(parameters);
          }
        }

        return new ProcessorOutput("empty", new object());
      }
      catch (Exception ex)
      {
        Logger.Error("Failed to access Rejseplanen", ex);
        return new ProcessorOutput("error", new object());
      }
    }


    public static ProcessorOutput FindNextDepartures(ProcessorInput input)
    {
      System.Threading.Thread.Sleep(3000);
      string station = input.GetParameter<string>(0);
      string types = input.GetOptionalParameter<string>(1, "toget bussen metro");

      var parameters = new Dictionary<string, object>();

      Logger.Debug($"Looking for departures from '{station}' in Rejseplanen");

      try
      {
        RejseplanenAPI api = new RejseplanenAPI();
        LocationList locations = api.GetLocations(station);

        StopLocation location = locations.Items.OfType<StopLocation>().FirstOrDefault();
        if (location != null)
        {
          DepartureBoard departures = api.GetDepartureBoard(location.id, types);
          if (departures != null && departures.Departure != null)
          {
            var result = departures.Departure.Select(d =>
              new
              {
                date = d.date,
                time = d.time,
                direction = d.direction,
                line = d.name
              }).Take(5).ToList();

            parameters["stop"] = location.name;
            parameters["result"] = result;
            return new ProcessorOutput(parameters);
          }
        }

        return new ProcessorOutput("empty", new object());
      }
      catch (Exception ex)
      {
        Logger.Error("Failed to access Rejseplanen", ex);
        return new ProcessorOutput("error", new object());
      }
    }
  }
}

