using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Processors
{
  public static class GeneralProcessors
  {
    public static void RegisterProcessors()
    {
      ProcessorRegistry.RegisterProcessor("General.Echo", Echo);
    }


    public static object Echo(ProcessorInput input)
    {
      string result = input.GetParameter<string>(0);
      return new { result = result };
    }
  }
}
