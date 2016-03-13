using System;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class SelfProcessors
  {
    public static Func<string> KnownDomains(KnowledgeBase kb, string template)
    {
      object answer = kb.GetDomains().Select(d => d.Name);

      return () => TextMerge.MergeTemplate(template, new { answer = answer });
    }
  }
}
