using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Model;
using TimeFillets.Connectors;

namespace TimeFillets.Tests
{
  class TestCalendarConnector : ConnectorBase, ICalendarConnector
  {
    public List<CalendarItem> Items;


    public TestCalendarConnector()
    {
      Items = new List<CalendarItem>();
      Items.Add(new CalendarItem(new Customer(), new Project(), new Task()));
      Items.Add(new CalendarItem(new Customer(), new Project(), new Task()));
      Items.Add(new CalendarItem(new Customer(), new Project(), new Task()));
    }

    #region ICalendarConnector Members

    public IEnumerable<CalendarItem> GetCalendarItems()
    {
      return Items;
    }

    public IEnumerable<CalendarItem> GetCalendarItems(DateTime from, DateTime to)
    {
      return Items;
    }

    public IEnumerable<CalendarItem> GetCalendarItems(Project project)
    {
      return Items;
    }

    public IEnumerable<CalendarItem> GetCalendarItems(Customer customer)
    {
      return Items;
    }

    public IEnumerable<CalendarItem> GetCalendarItems(Task task)
    {
      return Items;
    }

    #endregion

    #region ICalendarConnector Members


    public void EditCalendarItem(CalendarItem item)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
