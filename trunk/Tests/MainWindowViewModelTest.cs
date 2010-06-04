using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeFillets.ViewModel;
using TimeFillets.Helpers;
using TimeFillets.Model;

namespace TimeFillets.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MainWindowViewModelTest
  {
    public MainWindowViewModelTest()
    {
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void RefreshCalendarTest()
    {
      CommandViewModel projectDefinitionsCommand = new CommandViewModel("Project definitions command", new EmptyCommand());
      CommandViewModel settingsCommand = new CommandViewModel("Settings command", new EmptyCommand());
      CommandViewModel itemDetailCommand = new CommandViewModel("Item detail command", new EmptyCommand());
      IErrorHelper errorHelper = new EmptyErrorHelper();
      ICalendarConnector calendarConnector = new TestCalendarConnector();
      MainWindowViewModel target = new MainWindowViewModel(calendarConnector, projectDefinitionsCommand, settingsCommand, itemDetailCommand, errorHelper);
      target.RefreshCalendarCommand.Command.Execute(null);
      Assert.AreEqual<int>(((TestCalendarConnector)calendarConnector).Items.Count, target.CalendarItems.Count);
    }
  }
}
