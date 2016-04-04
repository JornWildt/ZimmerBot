using System.IO;
using System.Text;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class ConfigurationParser
  {
    public void ParseConfigurationString(KnowledgeBase kb, string s)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(s, nameof(s)).IsNotNull();

      if (!s.EndsWith("\n"))
        s += "\n";

      ConfigParser parser = new ConfigParser(kb);
      parser.Parse(s);
    }


    public void ParseConfigurationFromFile(KnowledgeBase kb, string filename)
    {
      Condition.Requires(kb, nameof(kb)).IsNotNull();
      Condition.Requires(filename, nameof(filename)).IsNotNull();

      using (Stream s = new FileStream(filename, FileMode.Open))
      {
        ConfigParser parser = new ConfigParser(kb);
        parser.Parse(s, filename);
      }
    }
  }
}
