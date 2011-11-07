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
        return Math.Round(DurationSummary.TotalHours, 2);
      }
    }

    public double DurationSummaryManDays
    {
      get
      {
        return Math.Round(DurationSummaryHours / 8, 2);
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
