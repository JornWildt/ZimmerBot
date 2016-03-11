using System;
using System.Collections.Generic;


namespace ZimmerBot.Core.Responses
{
  public class MovieResponseGenerator : ResponseGenerator
  {
    string Lookup;


    public MovieResponseGenerator(string lookup)
    {
      Lookup = lookup;
    }


    public override Func<string> Bind(Dictionary<string, string> input)
    {
      return () => "Lookup movie by : " + input["name"] + " (find " + Lookup + ")";
    }
  }
}
