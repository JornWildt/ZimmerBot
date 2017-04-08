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
    #region Output template usage statistics

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

    #endregion


    #region Latest input handling

    const string LatestInput_SessionKey = "LatestInput";

    public static void RegisterLatestInput(this Session session, string input)
    {
      session.Store[LatestInput_SessionKey] = input;
    }


    public static string GetLatestInput(this Session session)
    {
      return session.Store[LatestInput_SessionKey];
    }

    #endregion


    #region Busy handling

    const string IsBusyWriting_SessionKey = "IsBusyWriting";

    public static void MarkAsBusyWritingAndNotReadyForInput(this Session session)
    {
      session.Store[IsBusyWriting_SessionKey] = true;
    }


    public static bool IsBusyWriting(this Session session)
    {
      return session.Store.ContainsKey(IsBusyWriting_SessionKey) && session.Store[IsBusyWriting_SessionKey];
    }


    public static void MarkAsReadyForInput(this Session session)
    {
      session.Store[IsBusyWriting_SessionKey] = false;
    }

    #endregion
  }
}
