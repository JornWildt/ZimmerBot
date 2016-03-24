using System.IO;
using System.Text;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.ConfigParser
{
  public class ConfigurationParser
  {
    public void ParseConfigurationString(Domain d, string s)
    {
      if (!s.EndsWith("\n"))
        s += "\n";
      ConfigParser parser = new ConfigParser(d);
      parser.Parse(s);
    }


    public void ParseConfigurationFromFile(Domain d, string filename)
    {
      using (Stream s = new FileStream(filename, FileMode.Open))
      {
        ConfigParser parser = new ConfigParser(d);
        parser.Parse(s);
      }
    }
  }
}
