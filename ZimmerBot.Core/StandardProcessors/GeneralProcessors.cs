using ZimmerBot.Core.Processors;


namespace ZimmerBot.Core.StandardProcessors
{
  public static class GeneralProcessors
  {
    public static void Initialize()
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
