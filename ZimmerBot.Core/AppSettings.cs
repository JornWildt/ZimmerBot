using System;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core
{
  public static class AppSettings
  {
    public static readonly AppSetting<string> LanguageDirectory = new AppSetting<string>("ZimmerBot.LanguageDirectory", ".");
    public static readonly AppSetting<string> Language = new AppSetting<string>("ZimmerBot.Language");
    public static readonly AppSetting<bool> EnableStemming = new AppSetting<bool>("ZimmerBot.EnableStemming", true);
    public static readonly AppSetting<bool> EnableSpellingCorrections = new AppSetting<bool>("ZimmerBot.EnableSpellingCorrections", true);
    public static readonly AppSetting<string> IntentKey = new AppSetting<string>("ZimmerBot.IntentKey", "intent");

    public static readonly AppSetting<string> RDF_ImportDirectory = new AppSetting<string>("ZimmerBot.RDF.ImportDirectory");
    public static readonly AppSetting<string> RDF_DataDirectory = new AppSetting<string>("ZimmerBot.RDF.DataDirectory");
    public static readonly AppSetting<string> RDF_BaseUrl = new AppSetting<string>("ZimmerBot.RDF.BaseUrl", "http://zimmerbot.org/");
    public static readonly AppSetting<string> RDF_ResourceUrl = new AppSetting<string>("ZimmerBot.RDF.ResourceUrl", "http://zimmerbot.org/resource/");
    public static readonly AppSetting<string> RDF_PropertyUrl = new AppSetting<string>("ZimmerBot.RDF.PropertyUrl", "http://zimmerbot.org/property/");
    public static readonly AppSetting<string> RDF_StaticStoreUrl = new AppSetting<string>("ZimmerBot.RDF.StaticStoreUrl", "http://zimmerbot.org/static-store/");
    public static readonly AppSetting<string> RDF_DynamicStoreUrl = new AppSetting<string>("ZimmerBot.RDF.DynamicStoreUrl", "http://zimmerbot.org/dynamic-store/");
    public static readonly AppSetting<bool> RDF_EnableChatLog = new AppSetting<bool>("ZimmerBot.RDF.EnableChatLog", true);
    public static readonly AppSetting<TimeSpan> RDF_BackupInterval = new AppSetting<TimeSpan>("ZimmerBot.RDF.BackupInterval", TimeSpan.FromMinutes(5));

    public static readonly AppSetting<int> MaxRecursionCount = new AppSetting<int>("ZimmerBot.MaxRecursionCount", 20);
    public static readonly AppSetting<TimeSpan> MessageSequenceDelay = new AppSetting<TimeSpan>("ZimmerBot.MessageSequenceDelay", TimeSpan.FromSeconds(0.05));
    public static readonly AppSetting<string> MessageSequenceNotificationText = new AppSetting<string>("ZimmerBot.MessageSequenceNotificationText", " (...)", trim: AppSetting.TrimSetting.NoTrim);

    public static Func<string, string> MapServerPath { get; set; } = (s => s);
  }
}
