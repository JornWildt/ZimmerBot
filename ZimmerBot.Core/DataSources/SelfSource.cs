using System;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataSources
{
  public static class SelfSource
  {
    public static Func<string> KnownDomains(KnowledgeBase kb, string template)
    {
      object answer = kb.GetDomains().Select(d => d.Name);

      return () => TextMerge.MergeTemplate(template, new { answer = answer });
    }
  }
}
