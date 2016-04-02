using log4net;
using NUnit.Framework;
using ZimmerBot.Core.StandardProcessors;


namespace ZimmerBot.Core.Tests
{
  [SetUpFixture]
  public class SetupFixture
  {
    static ILog Logger = LogManager.GetLogger(typeof(SetupFixture));

    static bool Initialized = false;

    [SetUp]
    public void Setup()
    {
      if (Initialized)
        return;

      // Start logging
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("**** STARTING ZimmerBot tests ****");

      GeneralProcessor.Initialize();
      DateTimeProcessor.Initialize();
      RDFProcessor.Initialize();

      Initialized = true;
    }
  }
}
