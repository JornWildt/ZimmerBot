using System;

namespace ZimmerBot.Core
{
  public static class UrlConstants
  {
    public static Uri TypeUrl(string type) => CreateUrl("types/" + type);

    public static Uri UsersUrl(string id) => CreateUrl("users/" + id);
    public static readonly Uri UserValuesUrl = CreateUrl("user/values");
    public static readonly Uri UserTypeUrl = TypeUrl("user");

    public static Uri SessionsUrl(string id) => CreateUrl("sessions/" + id);
    public static readonly Uri SessionTypeUrl = TypeUrl("session");

    public static readonly Uri BotUrl = CreateUrl("bot");
    public static readonly Uri BotValuesUrl = CreateUrl("bot/values");

    public static Uri ChatsUrl(string id) => CreateUrl("chats/" + id);
    public static readonly Uri ChatTypeUrl = TypeUrl("chat");

    public static readonly Uri ChatUrlBase = CreateUrl("chat/");
    public static Uri ChatEntriesUrl(string id) => CreateUrl("chat/entries/" + id);
    public static readonly Uri ChatEntryTypeUrl = TypeUrl("chatentry");

    public static readonly Uri ChatReferenceTypeUrl = TypeUrl("chatref");

    public static Uri ResourceUrl(string id) => new Uri(AppSettings.RDF_ResourceUrl + id);
    public static Uri PropertyUrl(string id) => new Uri(AppSettings.RDF_PropertyUrl + id);


    public static Uri Rdf(string key) => new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#" + key);

    public static Uri Rdfs(string key) => new Uri("http://www.w3.org/2000/01/rdf-schema#" + key);

    public static Uri DcTerms(string key) => new Uri("http://purl.org/dc/terms/" + key);

    public static Uri Foaf(string key) => new Uri("http://xmlns.com/foaf/0.1/" + key);

    public static Uri CreateUrl(string path) => new Uri(AppSettings.RDF_BaseUrl + path);
  }
}
