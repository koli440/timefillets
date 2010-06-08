using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Model;
using System.ComponentModel;

namespace TimeFillets.Model
{
  public interface IExportConnector
  {
    string ConnectorName { get; }
    void Export(IEnumerable<CalendarItem> calendarItems, string path);
    BackgroundWorker Worker { get; set; }
  }
}
