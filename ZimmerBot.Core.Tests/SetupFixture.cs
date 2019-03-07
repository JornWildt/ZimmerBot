using System;
using System.IO;
using log4net;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.StandardProcessors;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Tests
{
  [SetUpFixture]
  public class SetupFixture
  {
    static ILog Logger = LogManager.GetLogger(typeof(SetupFixture));

    static bool Initialized = false;

    [OneTimeSetUp]
    public void Setup()
    {
      if (Initialized)
        return;

      // Start logging
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("**** STARTING ZimmerBot tests ****");

      // Some tests and data loading depends on this (current directory should be the test file (.dll) location => ...\bin\Debug\x.dll)
      Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

      GeneralProcessor.Initialize();
      DateTimeProcessor.Initialize();
      RDFProcessor.Initialize();
      CryptoHelper.Initialize();
      SpellChecker.Initialize();
      TextMerge.Initialize();

      TextMerge.LoadFromFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\BotTests"));

      Initialized = true;
    }


    [OneTimeTearDown]
    public void TearDown()
    {
      CryptoHelper.Shutdown();
      ZimmerBotConfiguration.Shutdown();
    }
  }
}
