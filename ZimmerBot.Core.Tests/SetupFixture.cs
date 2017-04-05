﻿using System;
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

      Initialized = true;
    }


    [OneTimeTearDown]
    public void TearDown()
    {
      ZimmerBotConfiguration.Shutdown();
    }
  }
}
