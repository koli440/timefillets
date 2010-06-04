using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Model;
using TimeFillets.Helpers;

namespace TimeFillets.ViewModel
{
  public class EventDetailWindowViewModel : ViewModelBase
  {
    private const string timeFormatString = "HH:mm";

    private CalendarItem _eventItem;
    private CommandViewModel _saveCommand;
    private ICalendarConnector _calendarConnector;
    private IErrorHelper _errorHelper;

    #region Properties
    public CalendarItem EventItem
    {
      get { return _eventItem; }
      set
      {
        if (value == _eventItem)
          return;

        _eventItem = value;
        base.OnPropertyChanged("EventItem");
      }
    }

    public CommandViewModel SaveCommand
    {
      get 
      {
        if (_saveCommand == null)
        {
          _saveCommand = new CommandViewModel("Save Event", new RelayCommand(param => this.Save()));
        }
        return _saveCommand;
      }
    }

    public string StartTime
    {
      get
      {
        if (EventItem.StartDate != null && EventItem.StartDate > DateTime.MinValue)
          return EventItem.StartDate.ToString(timeFormatString);
        return string.Empty;
      }
    }

    public string EndTime
    {
      get
      {
        if (EventItem.EndDate != null && EventItem.EndDate > DateTime.MinValue)
          return EventItem.EndDate.ToString(timeFormatString);
        return string.Empty;
      }
    }

    public string UpdatedTime
    {
      get
      {
        if (EventItem.Updated != null && EventItem.Updated > DateTime.MinValue)
          return EventItem.Updated.ToString(timeFormatString);
        return string.Empty;
      }
    }
    #endregion

    #region constructor

    public EventDetailWindowViewModel(CalendarItem eventItem, ICalendarConnector calendarConnector, IErrorHelper errorHelper)
    {
      if (eventItem == null)
        throw new ArgumentNullException("eventItem", "eventItem cannot be null");
      if (calendarConnector == null)
        throw new ArgumentNullException("calendarConnector", "connector cannot be null");
      if (errorHelper == null)
        throw new ArgumentNullException("errorHelper", "error helper cannot be null");

      this.EventItem = eventItem;
      this._calendarConnector = calendarConnector;
      this._errorHelper = errorHelper;
    }

    #endregion

    #region methods

    public void Save()
    {
      try
      {
        _calendarConnector.EditCalendarItem(this.EventItem);
        _errorHelper.ShowError("Calendar item successfully saved");
      }
      catch (Exception e)
      {
        _errorHelper.ShowError(e);
      }
    }

    #endregion
  }
}
