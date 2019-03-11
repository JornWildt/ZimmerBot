using System;
using System.Collections.Generic;
using System.Xml.Serialization;
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


    public Forecast GetForecast(string input, int hours)
    {
      ISession session = NewSession();

      WebRequest request = session.Bind("forecast", new
      {
        q = input,
        appid = OpenWeatherMapAppSettings.Key.Value,
        units = "metric",
        lang = "da",
        mode = "xml",
        cnt = (hours-1)/3 + 1 // One item for every 3 hours
      });

      using (var response = request.AcceptXml().Get<Forecast>())
      {
        return response.Body;
      }
    }


    [XmlRoot("weatherdata")]
    public class Forecast
    {
      [XmlElement("sun")]
      public ForecastSun Sun { get; set; }

      [XmlArray("forecast")]
      [XmlArrayItem("time")]
      public List<ForecastItem> List { get; set; }
    }


    public class ForecastSun
    {
      [XmlAttribute("rise")]
      public DateTime Rise { get; set; }

      [XmlAttribute("set")]
      public DateTime Set { get; set; }
    }


    public class ForecastItem
    {
      [XmlAttribute("from")]
      public DateTime From { get; set; }

      [XmlAttribute("to")]
      public DateTime To { get; set; }

      [XmlElement("symbol")]
      public ForecastSymbol Symbol { get; set; }

      [XmlElement("windDirection")]
      public ForecastWindDirection WindDirection { get; set; }

      [XmlElement("windSpeed")]
      public ForecastWindSpeed WindSpeed { get; set; }

      [XmlElement("temperature")]
      public ForecastTemperature Temperature { get; set; }

      [XmlElement("clouds")]
      public ForecastClouds Clouds { get; set; }

      [XmlElement("precipation")]
      public ForecastPrecipation Precipation { get; set; }
    }


    public class ForecastSymbol
    {
      [XmlAttribute("number")]
      public string Number { get; set; }

      [XmlAttribute("name")]
      public string Name { get; set; }

      [XmlAttribute("var")]
      public string Var { get; set; }
    }


    public class ForecastWindDirection
    {
      [XmlAttribute("deg")]
      public string Deg { get; set; }

      [XmlAttribute("code")]
      public string Code { get; set; }

      [XmlAttribute("name")]
      public string Name { get; set; }
    }


    public class ForecastWindSpeed
    {
      [XmlAttribute("mps")]
      public decimal MetersPerSecond { get; set; }

      [XmlAttribute("name")]
      public string Name { get; set; }
    }


    public class ForecastTemperature
    {
      [XmlAttribute("value")]
      public decimal Value { get; set; }

      [XmlAttribute("min")]
      public decimal Min { get; set; }

      [XmlAttribute("max")]
      public decimal Max { get; set; }
    }


    public class ForecastClouds
    {
      [XmlAttribute("value")]
      public string Value { get; set; }

      [XmlAttribute("all")]
      public decimal all { get; set; }
    }


    public class ForecastPrecipation
    {
      [XmlAttribute("value")]
      public decimal Value { get; set; }

      [XmlAttribute("type")]
      public string type { get; set; }
    }
  }
}
