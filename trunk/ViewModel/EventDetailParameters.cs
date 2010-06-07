using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Model;

namespace TimeFillets.ViewModel
{
  public class EventDetailParameters
  {
    public CalendarItem SelectedItem { get; set; }
    public MainWindowViewModel MainWindow { get; set; }
  }
}
