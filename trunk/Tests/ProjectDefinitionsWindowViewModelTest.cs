using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeFillets.ViewModel;
using System.Xml.Linq;
using TimeFillets.Helpers;

namespace TimeFillets.Tests
{
  [TestClass]
  public class ProjectDefinitionsWindowViewModelTest
  {
    [TestMethod]
    public void CanScanDefinition()
    {
      IErrorHelper errorHelper = new EmptyErrorHelper();
      ProjectDefinitionsWindowViewModel target = new ProjectDefinitionsWindowViewModel(errorHelper);
      target.ScanCommand.Command.Execute(null);
      Assert.IsNotNull(target.ScannedDefinitions);
    }

    [TestMethod]
    public void CanSaveDefinition()
    {
      IErrorHelper errorHelper = new EmptyErrorHelper();
      XDocument doc = XDocument.Parse("<?xml version=\"1.0\" encoding=\"utf-8\" ?><projectsDefinitions></projectsDefinitions>");
      ProjectDefinitionsWindowViewModel target = new ProjectDefinitionsWindowViewModel(errorHelper);
      target.ScannedDefinitions = doc;
      target.SaveCommand.Command.Execute(null);
    }
  }
}
