using NUnit.Framework;


namespace ZimmerBot.Core.Tests
{
  public class TestHelper
  {
    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [OneTimeSetUp]
    public void MasterTestFixtureSetUp()
    {
      TestFixtureSetUp();
    }


    /// <summary>
    /// Executed only once before all tests. Override in subclasses to do subclass
    /// set up. Remember to call base.TestFixtureSetUp().
    /// NOTE: The [TestFixtureSetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void TestFixtureSetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [SetUp]
    public void MasterSetUp()
    {
      SetUp();
    }


    /// <summary>
    /// Executed before each test method is run. Override in subclasses to do subclass
    /// set up. Remember to call base.SetUp().
    /// NOTE: The [SetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void SetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TearDown]
    public void MasterTearDown()
    {
      TearDown();
    }

    /// <summary>
    /// Executed after each test method is run.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TearDown().
    /// NOTE: [TearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TearDown()
    {
    }

    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [OneTimeTearDown]
    public void TestFixtureMasterTearDown()
    {
      TestFixtureTearDown();
    }

    /// <summary>
    /// Executed only once after all tests.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TestFixtureTearDown().
    /// NOTE: [TestFixtureTearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TestFixtureTearDown()
    {
    }
  }
}
