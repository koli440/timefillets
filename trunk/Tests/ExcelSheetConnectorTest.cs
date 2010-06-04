using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeFillets.Connectors;
using TimeFillets.Model;

namespace TimeFillets.Tests
{
  [TestClass]
  public class ExcelSheetConnectorTest
  {
    [TestMethod]
    public void CanExport()
    {
      List<CalendarItem> items = new List<CalendarItem>();
      CalendarItem item1 = new CalendarItem(new Customer() { Name = "Test Customer 1" }, new Project() { Name = "Test Project 1" }, new Task() { Name = "Test Task 1" });
      item1.StartDate = DateTime.Now.AddDays(-1);
      item1.EndDate = DateTime.Now.AddDays(-1).AddHours(3);
      item1.Title = "Test Title 1";
      item1.Description = "Test description 1";
      items.Add(item1);

      CalendarItem item2 = new CalendarItem(new Customer() { Name = "Test Customer 2" }, new Project() { Name = "Test Project 2" }, new Task() { Name = "Test Task 2" });
      item2.StartDate = DateTime.Now.AddDays(-3);
      item2.EndDate = DateTime.Now.AddDays(-3).AddHours(8);
      item2.Title = "Test Title 2";
      item2.Description = "Test description 2";
      items.Add(item2);

      CalendarItem item3 = new CalendarItem(new Customer() { Name = "Test Customer 3" }, new Project() { Name = "Test Project 3" }, new Task() { Name = "Test Task 3" });
      item3.StartDate = DateTime.Now.AddDays(-5);
      item3.EndDate = DateTime.Now.AddDays(-5).AddHours(1.5);
      item3.Title = "Test Title 3";
      item3.Description = "Test description 3";
      items.Add(item3);

      ExcelSheetConnector connector = new ExcelSheetConnector("F:\\Backup\\Projekty\\GoogleCalendarTimeSheet\\GoogleCalendarTimeSheet\\TimesheetTemplate.xlsx");
      connector.Export(items, "F:\\test.xlsx");
    }
  }
}
