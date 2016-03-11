using System;
using System.Collections.Generic;


namespace ZimmerBot.Core.Responses
{
  public class WeekDayResponseGenerator : ResponseGenerator
  {
    string Lookup;


    public WeekDayResponseGenerator(string lookup)
    {
      Lookup = lookup;
    }


    public override Func<string> Bind(Dictionary<string, string> input)
    {
      var culture = new System.Globalization.CultureInfo("da-DK");
      var today = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

      string day = input["name"];
      return () =>
        today.Equals(day, StringComparison.CurrentCultureIgnoreCase)
        ? "Ja"
        : "Nej";
    }
  }
}
