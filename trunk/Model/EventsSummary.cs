using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeFillets.Model
{
  public class EventsSummary
  {
    public TimeSpan DurationSummary { get; set; }
    public double DurationSummaryHours
    {
      get
      {
        return DurationSummary.TotalHours;
      }
    }

    public double DurationSummaryManDays
    {
      get
      {
        return DurationSummaryHours / 8;
      }
    }
    public int EventsCount { get; set; }
    public int ProjectsCount { get; set; }
    public int CustomersCount { get; set; }
    public int TasksCount { get; set; }

    public EventsSummary()
    {
      DurationSummary = new TimeSpan();
    }
  }
}
