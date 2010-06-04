using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TimeFillets.Model;
using TimeFillets.Connectors;
using TimeFillets.Helpers;
using System.Diagnostics;

namespace TimeFillets.ViewModel
{
  /// <summary>
  /// View model for project definitons window
  /// </summary>
  public class ProjectDefinitionsWindowViewModel : ViewModelBase
  {
    #region fields
    private XDocument _scannedDefinitions;
    #endregion

    #region properties
    /// <summary>
    /// Scanned definitions
    /// </summary>
    public XDocument ScannedDefinitions
    {
      get { return _scannedDefinitions; }
      set
      {
        if (value == _scannedDefinitions)
          return;

        _scannedDefinitions = value;
        base.OnPropertyChanged("ScannedDefinitions");
        base.OnPropertyChanged("ScannedDefinitionsFormated");
      }
    }

    /// <summary>
    /// Formated definitions for databinding
    /// </summary>
    public string ScannedDefinitionsFormated
    {
      get
      {
        if (ScannedDefinitions == null)
          return string.Empty;
        return ScannedDefinitions.ToString();
      }
      set
      {
        if (value == ScannedDefinitions.ToString())
          return;

        ScannedDefinitions = XDocument.Parse(value);
        base.OnPropertyChanged("ScannedDefinitionsFormated");
      }
    }

    /// <summary>
    /// Command for scaning
    /// </summary>
    public CommandViewModel ScanCommand { get; private set; }

    /// <summary>
    /// Command for saving
    /// </summary>
    public CommandViewModel SaveCommand { get; private set; }

    public IErrorHelper ErrorHelper { get; set; }
    #endregion

    #region constructor
    /// <summary>
    /// Default constructor
    /// </summary>
    public ProjectDefinitionsWindowViewModel(IErrorHelper errorHelper)
    {
      ScanCommand = new CommandViewModel("Scan definitions", new RelayCommand(param => this.ScanDefinitions()));
      SaveCommand = new CommandViewModel("Save definitions", new RelayCommand(param => this.SaveDefinitions()));
      ErrorHelper = errorHelper;
      ProjectDefinitionsConnector connector = new ProjectDefinitionsConnector();
      ScannedDefinitions = connector.OpenDefinitions();
    }
    #endregion

    #region methods
    /// <summary>
    /// Saves definitions.
    /// </summary>
    protected void SaveDefinitions()
    {
      try
      {
        ProjectDefinitionsConnector connector = new ProjectDefinitionsConnector();
        connector.SaveDefinitions(ScannedDefinitions);
        ErrorHelper.ShowError("Project definitions saved");
      }
      catch (Exception e)
      {
        Trace.WriteLine(e);
        ErrorHelper.ShowError(e);
      }
    }

    /// <summary>
    /// Scans for definitions
    /// </summary>
    protected void ScanDefinitions()
    {
      try
      {
        ProjectDefinitionsConnector connector = new ProjectDefinitionsConnector();
        this.ScannedDefinitions = connector.ScanForDefinitions();
      }
      catch (Exception e)
      {
        Trace.WriteLine(e);
        ErrorHelper.ShowError(e);
      }
    }
    #endregion

  }
}
