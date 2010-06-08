using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;

using TimeFillets.Model;
using TimeFillets.Connectors;
using TimeFillets.Helpers;
using TimeFillets.Logic;

namespace TimeFillets.ViewModel
{
  /// <summary>
  /// View model for main window
  /// </summary>
  public class MainWindowViewModel : ViewModelBase
  {
    #region fields
    private ObservableCollection<CalendarItem> _calendarItems;
    private DateTime? _from;
    private DateTime? _to;
    private CommandViewModel _refreshCalendarCommand;
    private CommandViewModel _projectDefinitionsCommand;
    private CommandViewModel _settingsCommand;
    private CommandViewModel _searchCommand;
    private CommandViewModel _itemDetailCommand;
    private CommandViewModel _exportCommand;
    private CommandViewModel _progressCommand;
    private string _searchPhrase;
    private string _searchIn = "Title";
    private string _exportType = ".xlsx";
    private string _exportPath;
    #endregion

    #region private variables
    protected ICalendarConnector _calendarConnector;
    private BackgroundWorker _worker;
    #endregion

    #region properties

    /// <summary>
    /// Collection with returned calendar items
    /// </summary>
    public ObservableCollection<CalendarItem> CalendarItems
    {
      get
      {
        if (_calendarItems == null)
        {
          _calendarItems = new ObservableCollection<CalendarItem>();
        }
        return _calendarItems;
      }
    }

    /// <summary>
    /// Begining of selection time range
    /// </summary>
    public DateTime? From
    {
      get { return _from; }
      set
      {
        if (value == _from)
          return;

        _from = value;
        base.OnPropertyChanged("From");
      }
    }

    /// <summary>
    /// End of selection time range
    /// </summary>
    public DateTime? To
    {
      get { return _to; }
      set
      {
        if (value == _to)
          return;

        _to = value;
        base.OnPropertyChanged("To");
      }
    }

    /// <summary>
    /// Command for refreshing items
    /// </summary>
    public CommandViewModel RefreshCalendarCommand
    {
      get
      {
        if (_refreshCalendarCommand == null)
          _refreshCalendarCommand = new CommandViewModel("Refresh Calendar", new RelayCommand(param => this.RefreshCalendarAsync()));
        return _refreshCalendarCommand;
      }
    }

    /// <summary>
    /// Command for displaying project definitions
    /// </summary>
    public CommandViewModel ProjectDefinitionsCommand
    {
      get { return _projectDefinitionsCommand; }
      private set { _projectDefinitionsCommand = value; }
    }

    /// <summary>
    /// Command for exporting data
    /// </summary>
    public CommandViewModel ExportCommand
    {
      get
      {
        if (_exportCommand == null)
          _exportCommand = new CommandViewModel("Export", new RelayCommand(param => this.ExportAsync()));
        return _exportCommand;
      }
    }

    /// <summary>
    /// Command for displaying application settings
    /// </summary>
    public CommandViewModel SettingsCommand
    {
      get { return _settingsCommand; }
      private set { _settingsCommand = value; }
    }

    /// <summary>
    /// Command for search in items
    /// </summary>
    public CommandViewModel SearchCommand
    {
      get
      {
        if (_searchCommand == null)
          _searchCommand = new CommandViewModel("Search", new RelayCommand(param => this.SearchAsync()));
        return _searchCommand;
      }

    }

    /// <summary>
    /// Command for openning item detail
    /// </summary>
    public CommandViewModel ItemDetailCommand
    {
      get { return _itemDetailCommand; }
      private set { _itemDetailCommand = value; }
    }

    /// <summary>
    /// Command to dispaly operation progress
    /// </summary>
    public CommandViewModel ProgressCommand
    {
      get { return _progressCommand; }
      private set { _progressCommand = value; }
    }

    /// <summary>
    /// Search value
    /// </summary>
    public string SearchPhrase
    {
      get { return _searchPhrase; }
      set
      {
        if (value == _searchPhrase)
          return;

        _searchPhrase = value;
        base.OnPropertyChanged("SearchPhrase");
      }
    }

    /// <summary>
    /// Search in column
    /// </summary>
    public string SearchIn
    {
      get { return _searchIn; }
      set
      {
        if (_searchIn == value)
          return;

        _searchIn = value;
        base.OnPropertyChanged("SearchIn");
      }
    }

    /// <summary>
    /// Helps with displaying errors
    /// </summary>
    public IErrorHelper ErrorHelper { get; set; }

    /// <summary>
    /// Connectors for export
    /// </summary>
    public List<IExportConnector> ExportConnectors { get; set; }

    /// <summary>
    /// Typ exportu - Excel, pdf ...
    /// </summary>
    public string ExportType
    {
      get { return _exportType; }
      set
      {
        if (value == _exportType)
          return;

        _exportType = value;
        base.OnPropertyChanged("ExportType");
      }
    }

    /// <summary>
    /// Path of file to export
    /// </summary>
    public string ExportPath
    {
      get { return _exportPath; }
      set
      {
        if (value == _exportPath)
          return;

        _exportPath = value;
        base.OnPropertyChanged("ExportPath");
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="calendarConnector">Connector for calendar</param>
    /// <param name="projectDefinitionsCommand">Command which displays project definitions</param>
    /// <param name="settingsCommand">Command for opening a settings window</param>
    /// <param name="itemDetailCommand">Command for opening a event detail window</param>
    /// <param name="errorHelper">Helper for displaying errors and messages</param>
    public MainWindowViewModel(ICalendarConnector calendarConnector, CommandViewModel projectDefinitionsCommand, CommandViewModel settingsCommand, CommandViewModel itemDetailCommand, CommandViewModel progressCommand, IErrorHelper errorHelper)
    {
      if (calendarConnector == null)
        throw new ArgumentException("Calendar connector cannot be null", "calendarConnector");
      if (projectDefinitionsCommand == null)
        throw new ArgumentException("Project definitions command cannot be null", "projectDefinitionsCommand");
      if (settingsCommand == null)
        throw new ArgumentException("Settings command cannot be null", "settingsCommand");
      if (errorHelper == null)
        throw new ArgumentException("Error helper cannot be null", "errorHelper");
      if (itemDetailCommand == null)
        throw new ArgumentException("Item detail command cannot be null", "itemDetailCommand");
      if (progressCommand == null)
        throw new ArgumentException("Progress command cannot be null", "progressCommand");

      _calendarConnector = calendarConnector;
      ProjectDefinitionsCommand = projectDefinitionsCommand;
      SettingsCommand = settingsCommand;
      ItemDetailCommand = itemDetailCommand;
      ProgressCommand = progressCommand;
      ErrorHelper = errorHelper;

      _worker = new BackgroundWorker();
      _worker.WorkerReportsProgress = true;

      _calendarConnector.Worker = _worker;

      if (SettingsConnector.ApplicationSettings.IsConfigurationValid)
      {
        RefreshCalendarAsync();
      }
    }

    #endregion

    #region events
    public void CalendarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {

    }
    #endregion

    #region method
    /// <summary>
    /// Refreshes calendar items with selected time range
    /// </summary>
    public IEnumerable<CalendarItem> RefreshCalendar()
    {
      IEnumerable<CalendarItem> ret = null;
      try
      {
        if (_calendarConnector != null)
        {
          if (From != null && From != null)
            ret = _calendarConnector.GetCalendarItems((DateTime)From, (DateTime)To);
          else
            ret = _calendarConnector.GetCalendarItems();
        }
      }
      catch (Exception e)
      {
        Trace.WriteLine(e.Message);
        ErrorHelper.ShowError(e);
      }
      return ret;
    }

    public void RefreshCalendarAsync()
    {
      DoWorkEventHandler doWork = new DoWorkEventHandler((sender, args) => {
        args.Result = RefreshCalendar();
      });
      _worker.DoWork += doWork;

      RunWorkerCompletedEventHandler workCompleted = null;
       workCompleted = new RunWorkerCompletedEventHandler((sender, args) =>
      {
        _worker.DoWork -= doWork;
        _worker.RunWorkerCompleted -= workCompleted;
        CalendarItems.Clear();
        if (args.Result != null)
        {
          foreach (var item in (IEnumerable<CalendarItem>)args.Result)
          {
            CalendarItems.Add(item);
          }
        }
      });
      _worker.RunWorkerCompleted += workCompleted;

      ProgressCommand.Command.Execute(_worker);
    }

    /// <summary>
    /// Searches calandar with SearchPhrase and SearchIn properties asynchronously
    /// </summary>
    public void SearchAsync()
    {
      DoWorkEventHandler doWork = new DoWorkEventHandler((sender, args) =>
      {
        args.Result = Search(); 
      });
      _worker.DoWork += doWork;
      RunWorkerCompletedEventHandler workCompleted = null;
      workCompleted = new RunWorkerCompletedEventHandler((sender, args) =>
     {
       _worker.DoWork -= doWork;
       _worker.RunWorkerCompleted -= workCompleted;
       CalendarItems.Clear();
       if (args.Result != null)
       {
         foreach (var item in (IEnumerable<CalendarItem>)args.Result)
         {
           CalendarItems.Add(item);
         }
       }

     });
      _worker.RunWorkerCompleted += workCompleted;
      ProgressCommand.Command.Execute(_worker);
    }

    /// <summary>
    /// Searches calandar with SearchPhrase and SearchIn properties
    /// </summary>
    public IEnumerable<CalendarItem> Search()
    {
      if (string.IsNullOrEmpty(SearchPhrase))
      {
        return RefreshCalendar();
      }

      var ret = Searcher.Search(SearchIn, SearchPhrase, _calendarConnector.GetCalendarItems());

      if (this.From == null && this.To == null)
        return ret;

      return ret.Where(item => item.StartDate >= this.From && item.EndDate <= this.To);
    }

    /// <summary>
    /// Exports data from list to file asynchronously
    /// </summary>
    public void ExportAsync()
    {
      _worker.DoWork += (sender, args) => { Export(); };
      ProgressCommand.Command.Execute(_worker);
    }

    /// <summary>
    /// Exports data from list to file
    /// </summary>
    public void Export()
    {
      try
      {
        if (ExportType.ToLower() == ".xlsx")
        {
          if (ExportConnectors == null || ExportConnectors.Count == 0)
          {
            ErrorHelper.ShowError("There are no export providers avaible");
            return;
          }
          ExcelSheetConnector exportConnector = ExportConnectors.Where(itm => itm.ConnectorName == "ExcelExportConnector").FirstOrDefault() as ExcelSheetConnector;
          exportConnector.Worker = _worker;
          if (exportConnector != null)
          {
            if (string.IsNullOrWhiteSpace(ExportPath))
            {
              ErrorHelper.ShowError("Please specify a valid path for export");
              return;
            }

            exportConnector.Export(this.CalendarItems, ExportPath);
            ErrorHelper.ShowError(string.Format("Export to {0} succeded.", ExportPath));
          }
          else
          {
            ErrorHelper.ShowError(string.Format("There is no provider avaible for selected type of export ({0}).", ExportType));
          }
        }
        else
        {
          ErrorHelper.ShowError("Unsupported type of export");
        }
      }
      catch (Exception e)
      {
        ErrorHelper.ShowError(e);
      }
    }
    #endregion
  }
}
