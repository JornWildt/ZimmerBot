using System;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core
{
  public static class AppSettings
  {
    public static readonly AppSetting<string> RDF_ImportDirectory = new AppSetting<string>("ZimmerBot.RDF.ImportDirectory");
    public static readonly AppSetting<string> RDF_DataDirectory = new AppSetting<string>("ZimmerBot.RDF.DataDirectory");
    public static readonly AppSetting<string> RDF_BaseUrl = new AppSetting<string>("ZimmerBot.RDF.BaseUrl", "http://zimmerbot.org/");
    public static readonly AppSetting<bool> RDF_EnableChatLog = new AppSetting<bool>("ZimmerBot.RDF.EnableChatLog", true);
    public static readonly AppSetting<TimeSpan> RDF_BackupInterval = new AppSetting<TimeSpan>("ZimmerBot.RDF.BackupInterval", TimeSpan.FromMinutes(5));

    public static readonly AppSetting<TimeSpan> MessageSequenceDelay = new AppSetting<TimeSpan>("ZimmerBot.MessageSequenceDelay", TimeSpan.FromSeconds(0.05));
    public static readonly AppSetting<string> MessageSequenceNotoficationText = new AppSetting<string>("ZimmerBot.MessageSequenceNotoficationText", " (...)", trim: AppSetting.TrimSetting.NoTrim);

    public static Func<string, string> MapServerPath { get; set; } = (s => s);
  }
}
