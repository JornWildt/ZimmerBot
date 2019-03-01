using ZimmerBot.Core.Utilities;

namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenAppSettings
  {
    public static readonly ConnectionString<string>  RejseplanenUrl = new ConnectionString<string>("Rejseplanen.Url");
  }
}
