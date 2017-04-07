using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public static class SessionStateExtensions
  {
    static string UsageKey(string outputId) => "Usage_" + outputId;


    public static int GetUsageCount(this Session session, string outputId)
    {
      Condition.Requires(session, nameof(session)).IsNotNull();

      if (outputId == null)
        return 0;

      string key = UsageKey(outputId);

      if (!session.Store.ContainsKey(key))
        return 0;

      return session.Store[key];
    }


    public static void IncrementUsage(this Session session, string outputId)
    {
      Condition.Requires(session, nameof(session)).IsNotNull();

      if (outputId == null)
        return;

      string key = UsageKey(outputId);

      if (!session.Store.ContainsKey(key))
        session.Store[key] = 1;
      else
        session.Store[key] += 1;
    }


    public static void RegisterLatestInput(this Session session, string input)
    {
      session.Store["LatestInput"] = input;
    }


    public static string GetLatestInput(this Session session)
    {
      return session.Store["LatestInput"];
    }
  }
}
