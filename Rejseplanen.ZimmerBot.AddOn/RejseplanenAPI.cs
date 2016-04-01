using System;
using System.Configuration;
using log4net;
using Ramone;
using Ramone.MediaTypes.Xml;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using WebRequest = Ramone.Request;


namespace Rejseplanen.ZimmerBot.AddOn
{
  /// <summary>
  /// Implementation of Rejseplanen API.
  /// </summary>
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


    public DepartureBoard GetDepartureBoard(string locationId, string types)
    {
      ISession session = NewSession();

      DateTime now = DateTime.Now;
      string date = now.ToString("dd.MM.yy");
      string time = now.ToString("HH:mm");

      types = types.ToLower();
      int useTog = (types.Contains("tog") ? 1 : 0);
      int useBus = (types.Contains("bus") ? 1 : 0);
      int useMetro = (types.Contains("metro") ? 1 : 0);

      WebRequest request = session.Bind("departureBoard?id={id}&date={date}&time={time}&useTog={useTog}&useBus={useBus}&useMetro={useMetro}", 
        new { id = locationId, date = date, time = time, useTog = useTog, useBus = useBus, useMetro = useMetro });

      using (var response = request.AcceptXml().Get<DepartureBoard>())
      {
        return response.Body;
      }
    }
  }
}
