using System;
using System.Configuration;
using log4net;
using Ramone;
using Ramone.MediaTypes.Xml;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using WebRequest = Ramone.Request;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public class RejseplanenAPI
  {
    protected static ILog Logger = LogManager.GetLogger(typeof(RejseplanenAPI));

    protected IService RejseplanenService { get; private set; }


    public RejseplanenAPI()
    {
      if (ConfigurationManager.ConnectionStrings["Rejseplanen.Url"] == null)
        throw new InvalidOperationException($"Missing connection string 'Rejseplanen.Url'");

      string baseUrl = ConfigurationManager.ConnectionStrings["Rejseplanen.Url"].ConnectionString;
      RejseplanenService = RamoneConfiguration.NewService(new Uri(baseUrl));

      // This should rather have been in Ramone's standard codecs
      RejseplanenService.CodecManager.AddCodec<XmlSerializerCodec>(MediaType.TextXml);
    }


    public ISession NewSession()
    {
      ISession session = RejseplanenService.NewSession();
      return session;
    }


    public LocationList GetLocations(string input)
    {
      ISession session = NewSession();

      WebRequest request = session.Bind("location?input={i}", new { i = input });

      using (var response = request.AcceptXml().Get<LocationList>())
      {
        return response.Body;
      }
    }
  }
}
