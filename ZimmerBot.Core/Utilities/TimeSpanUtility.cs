using System;
using System.Linq;

namespace ZimmerBot.Core.Utilities
{
  public static class TimeSpanUtility
  {
    public static TimeSpan Parse(string time, TimeSpan defaultTime)
    {
      if (string.IsNullOrEmpty(time))
        return defaultTime;

      return Parse(time);
    }


    public static TimeSpan Parse(string time)
    {
      if (time == null)
        throw new ArgumentNullException(nameof(time));

      if (time.Contains(':'))
        return TimeSpan.Parse(time);
      else
        return TimeSpan.FromSeconds(int.Parse(time));
    }
  }
}
