﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Model;
using System.ComponentModel;

namespace TimeFillets.Model
{
  public interface ICalendarConnector
  {
    IEnumerable<CalendarItem> GetCalendarItems();
    IEnumerable<CalendarItem> GetCalendarItems(DateTime from, DateTime to);
    IEnumerable<CalendarItem> GetCalendarItems(Project project);
    IEnumerable<CalendarItem> GetCalendarItems(Customer customer);
    IEnumerable<CalendarItem> GetCalendarItems(Task task);

    void EditCalendarItem(CalendarItem item);

    void OnPropertyChanged(string propertyName);
    event PropertyChangedEventHandler PropertyChanged;
  }
}