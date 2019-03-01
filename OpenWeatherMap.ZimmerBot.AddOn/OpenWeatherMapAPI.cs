using System;
using System.Collections.Generic;
using log4net;
using Ramone;
using WebRequest = Ramone.Request;

namespace OpenWeatherMap.ZimmerBot.AddOn
{
  public class OpenWeatherMapAPI
  {
    protected static ILog Logger = LogManager.GetLogger(typeof(OpenWeatherMapAPI));

    protected IService OWMService { get; private set; }


    public OpenWeatherMapAPI()
    {
      string baseUrl = OpenWeatherMapAppSettings.BaseUrl;
      OWMService = RamoneConfiguration.NewService(new Uri(baseUrl));
    }


    public ISession NewSession()
    {
      ISession session = OWMService.NewSession();
      return session;
    }


    public Forecast GetForecast(string input)
    {
      ISession session = NewSession();

      WebRequest request = session.Bind("forecast?q={q}&units=metric&appid={key}", new { q = input, key = OpenWeatherMapAppSettings.Key.Value });

      using (var response = request.AcceptJson().Get<Forecast>())
      {
        return response.Body;
      }
    }


    public class Forecast
    {
      public List<ForecastList> list { get; set; }
    }


    public class ForecastList
    {
      public ForecastMain main { get; set; }
    }


    public class ForecastMain
    {
      public decimal temp { get; set; }
      public decimal temp_min { get; set; }
      public decimal temp_max { get; set; }
    }
  }
}
