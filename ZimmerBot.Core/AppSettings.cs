using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core
{
  public static class AppSettings
  {
    public static readonly AppSetting<string> RDF_ImportDirectory = new AppSetting<string>("ZimmerBot.RDF.ImportDirectory");
    public static readonly AppSetting<string> RDF_DataDirectory = new AppSetting<string>("ZimmerBot.RDF.DataDirectory");
  }
}
