using ZimmerBot.Core.Utilities;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public static class OpenWeatherMapAppSettings
  {
    public static readonly ConnectionString<string> Key = new ConnectionString<string>("OWM.ApiKey");
    public static readonly ConnectionString<string> BaseUrl = new ConnectionString<string>("OWM.BaseUrl");
  }
}
