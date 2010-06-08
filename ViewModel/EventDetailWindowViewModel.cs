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
    private CommandViewModel _deleteCommand;
    private ICalendarConnector _calendarConnector;
    private IErrorHelper _errorHelper;
    private MainWindowViewModel _mainWindow;

    #region Properties
    /// <summary>
    /// Item to be displayed and modified
    /// </summary>
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

    /// <summary>
    /// Command for saving changes
    /// </summary>
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

    /// <summary>
    /// Command for deleting item
    /// </summary>
    public CommandViewModel DeleteCommand
    {
      get
      {
        if (_deleteCommand == null)
        {
          _deleteCommand = new CommandViewModel("Delete Event", new RelayCommand(param => this.Delete()));
        }
        return _deleteCommand;
      }
    }

    /// <summary>
    /// Start time of Event
    /// </summary>
    public string StartTime
    {
      get
      {
        if (EventItem.StartDate != null && EventItem.StartDate > DateTime.MinValue)
          return EventItem.StartDate.ToString(timeFormatString);
        return string.Empty;
      }
      set
      {
        string dateTimeToParse = string.Format("{0} {1}", EventItem.StartDate.ToString("dd.MM.yyyy"), value);
        EventItem.StartDate = DateTime.Parse(dateTimeToParse);
        OnPropertyChanged("EventItem");
      }
    }

    /// <summary>
    /// End time of Event
    /// </summary>
    public string EndTime
    {
      get
      {
        if (EventItem.EndDate != null && EventItem.EndDate > DateTime.MinValue)
          return EventItem.EndDate.ToString(timeFormatString);
        return string.Empty;
      }
      set
      {
        string dateTimeToParse = string.Format("{0} {1}", EventItem.EndDate.ToString("dd.MM.yyyy"), value);
        EventItem.EndDate = DateTime.Parse(dateTimeToParse);
        OnPropertyChanged("EventItem");
      }
    }

    /// <summary>
    /// Update time of Event
    /// </summary>
    public string UpdatedTime
    {
      get
      {
        if (EventItem.Updated != null && EventItem.Updated > DateTime.MinValue)
          return EventItem.Updated.ToString(timeFormatString);
        return string.Empty;
      }
    }

    /// <summary>
    /// Mode of window - Update or Delete
    /// </summary>
    public EntityMode Mode { get; private set; }

    #endregion

    #region constructor

    public EventDetailWindowViewModel(CalendarItem eventItem, ICalendarConnector calendarConnector, IErrorHelper errorHelper, MainWindowViewModel mainWindow)
    {
      if (eventItem == null)
        throw new ArgumentNullException("eventItem", "eventItem cannot be null");
      if (calendarConnector == null)
        throw new ArgumentNullException("calendarConnector", "connector cannot be null");
      if (errorHelper == null)
        throw new ArgumentNullException("errorHelper", "error helper cannot be null");
      if (mainWindow == null)
        throw new ArgumentNullException("mainWindow", "reference to MainWindowViewModel cannot be null");

      this.EventItem = eventItem;
      this._mainWindow = mainWindow;
      if (string.IsNullOrWhiteSpace(eventItem.GoogleEventId))
      {
        this.Mode = EntityMode.Create;
      }
      else
        this.Mode = EntityMode.Update;
      this._calendarConnector = calendarConnector;
      this._errorHelper = errorHelper;

    }

    #endregion

    #region methods

    /// <summary>
    /// Saves (create or update) current instance of <see cref="TimeFillets.Model.CalendarItem"/> into store using connector.
    /// </summary>
    public void Save()
    {
      try
      {
        CalendarItem ret = null;
      
        string definitionString = ProjectSettings.CreateNewDefinitionString(_eventItem.CustomerItem.Name, _eventItem.ProjectItem.Name, _eventItem.TaskItem.Name);
        _eventItem.Description = string.Format("{0}\n{1}", _eventItem.CleanDescription, definitionString);

        if (this.Mode == EntityMode.Update)
          ret = _calendarConnector.EditCalendarItem(this.EventItem);
        if (this.Mode == EntityMode.Create)
          ret = _calendarConnector.CreateCalendarItem(this.EventItem);

        if (ret != null)
        {
          var oldEvent = _mainWindow.CalendarItems.Where(itm => itm.GoogleEventId == ret.GoogleEventId).FirstOrDefault();
          if (oldEvent == null)
          {
            _mainWindow.RefreshCalendar();
          }
          _errorHelper.ShowError("Calendar item successfully saved");
        }
        else
          _errorHelper.ShowError("Calendar item was not saved");

      }
      catch (Exception e)
      {
        _errorHelper.ShowError(e);
      }
    }

    /// <summary>
    /// Deletes this calendar item
    /// </summary>
    public void Delete()
    {
      try
      {
        if (_calendarConnector.DeleteCalendarItem(_eventItem))
        {
          _errorHelper.ShowError("Calendar item was deleted");
          _mainWindow.RefreshCalendar();
        }
        else
        {
          _errorHelper.ShowError("Calandar item failed to delete");
        }
      }
      catch (Exception e)
      {
        _errorHelper.ShowError(e);
      }

    }

    #endregion
  }
}
