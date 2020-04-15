using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using log4net;
using Nager.Date;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Processors;

namespace Holidays.ZimmerBot.AddOn
{
  public static class HolidaysProcessor
  {
    static ILog Logger = LogManager.GetLogger(typeof(HolidaysProcessor));


    public static ProcessorOutput DateOfHoliday(ProcessorInput input)
    {
      Logger.Debug($"Get date of holiday");

      string name = input.GetParameter<string>(0);

      var startDate = DateTime.Now.Date;
      var endDate = startDate.AddYears(1);
      var publicHolidays = DateSystem.GetPublicHoliday(startDate, endDate, CountryCode.DK);

      Logger.Debug($"Test for '{publicHolidays.Select(h => h.LocalName).Aggregate((a,b) => a + ", " + b)}'");

      // Get primary name ... should be a utility function somewhere
      string query = @"
select ?name
where
{
  ?subj rdfs:label ?name.
  ?subj zp:knownby ?knownby.
  FILTER (?knownby = lcase(@name))
}";

      RDFResultSet output = input.Context.KnowledgeBase.MemoryStore.Query(
        query,
        new Dictionary<string, object> { ["name"] = name },
        new List<object>());

      if (output.Count > 0)
        name = output[0]["name"] as string ?? name;

      foreach (var publicHoliday in publicHolidays)
      {
        string[] holidayNames = publicHoliday.LocalName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (holidayNames.Any(n => n.Trim().Equals(name, StringComparison.CurrentCultureIgnoreCase)))
        {
          Logger.Debug($"Matched '{publicHoliday.LocalName}'");
          Dictionary<string, object> result = new Dictionary<string, object>
          {
            ["date"] = publicHoliday.Date,
            ["days_left"] = (publicHoliday.Date - DateTime.Now.Date).Humanize()
          };

          return new ProcessorOutput(result);
        }
      }

      return new ProcessorOutput("empty", new Dictionary<string, object>());
    }
  }
}
