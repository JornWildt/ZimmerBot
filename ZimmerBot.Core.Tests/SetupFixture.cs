using NUnit.Framework;
using ZimmerBot.Core.StandardProcessors;


namespace ZimmerBot.Core.Tests
{
  [SetUpFixture]
  public class SetupFixture
  {
    private static bool Initialized = false;

    [SetUp]
    public void Setup()
    {
      if (Initialized)
        return;

      GeneralProcessor.Initialize();
      DateTimeProcessor.Initialize();

      Initialized = true;
    }
  }
}
