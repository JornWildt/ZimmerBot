using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ramone;
using Ramone.MediaTypes.Xml;
using Rejseplanen.ZimmerBot.AddOn.Schemas;
using WebRequest = Ramone.Request;
using WebResponse = Ramone.Response;


namespace Rejseplanen.ZimmerBot.AddOn
{
  public class RejseplanenAPI
  {
    protected static ILog Logger = LogManager.GetLogger(typeof(RejseplanenAPI));

    protected IService RejseplanenService { get; private set; }


    public RejseplanenAPI()
    {
      // FIXME: NO URL in GitHUB!!!!!
      RejseplanenService.CodecManager.AddXml<LocationList>(MediaType.TextXml);
      //RejseplanenService.CodecManager.AddCodec<XmlSerializerCodec>(MediaType.TextXml);
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

      using (var response = request.Get<LocationList>())
      {
        return response.Body;
      }
    }
  }
}
