using System.IO;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class ConfigurationParser
  {
    public void ParseConfiguration(Domain d, string s)
    {
      if (!s.EndsWith("\n"))
        s += "\n";
      ConfigParser parser = new ConfigParser(d);
      parser.Parse(s);
    }


    public void ParseConfigurationFromFile(Domain d, string filename)
    {
      // FIXME: make it stream based
      string s = File.ReadAllText(filename);
      ParseConfiguration(d, s);
    }
  }
}
