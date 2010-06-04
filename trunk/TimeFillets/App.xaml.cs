﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using TimeFillets.ViewModel;
using TimeFillets.Model;
using TimeFillets.Helpers;
using TimeFillets.Connectors;

namespace TimeFillets.MainApplication
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    /// <summary>
    /// Startup logic
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      MainWindow mainWindow = new MainWindow();

      CommandViewModel projectDefinitionsCommand = new CommandViewModel("Open project definitions", new RelayCommand(param => this.OpenProjectDefinitionsWindow()));
      CommandViewModel settingsCommand = new CommandViewModel("Open application settings", new RelayCommand(param => this.OpenSettingsWindow()));
      CommandViewModel itemDetailCommand = new CommandViewModel("Show item detail", new RelayCommand(param => this.OpenEventDetail((CalendarItem)param)));
      MessageBoxErrorHelper errorHelper = new MessageBoxErrorHelper();
      
      ICalendarConnector calendarConnector = new GoogleCalendarConnector(SettingsConnector.ApplicationSettings.UserName, SettingsConnector.ApplicationSettings.Password, SettingsConnector.ApplicationSettings.CalendarUrl, SettingsConnector.ApplicationSettings.ApplicationName);

      var viewModel = new MainWindowViewModel(calendarConnector, projectDefinitionsCommand, settingsCommand, itemDetailCommand, errorHelper);

      string excelTemplatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SoulFilets\\GoogleCalendarTimeSheet\\TimesheetTemplate.xlsx";
      IExportConnector excelExportConnector = new ExcelSheetConnector(excelTemplatePath);
      List<IExportConnector> exportConnectors = new List<IExportConnector>();
      exportConnectors.Add(excelExportConnector);
      viewModel.ExportConnectors = exportConnectors;

      mainWindow.DataContext = viewModel;
      mainWindow.Show();
    }

    /// <summary>
    /// Logic for opening a new project definition window.
    /// </summary>
    protected void OpenProjectDefinitionsWindow()
    {
      MessageBoxErrorHelper errorHelper = new MessageBoxErrorHelper();
      ProjectDefinitionsWindowViewModel viewModel = new ProjectDefinitionsWindowViewModel(errorHelper);
      ProjectsDefinitionsWindow projectDefinitionsWindow = new ProjectsDefinitionsWindow();
      projectDefinitionsWindow.DataContext = viewModel;
      projectDefinitionsWindow.ShowDialog();
    }

    /// <summary>
    /// Logic for opening window with application configuration.
    /// </summary>
    protected void OpenSettingsWindow()
    {
      SettingsWindowViewModel viewModel = new SettingsWindowViewModel();
      SettingsWindow settingsWindow = new SettingsWindow();
      settingsWindow.DataContext = viewModel;
      settingsWindow.ShowDialog();
    }

    protected void OpenEventDetail(CalendarItem selectedEvent)
    {
      if (selectedEvent != null)
      {
        MessageBoxErrorHelper errorHelper = new MessageBoxErrorHelper();
        ICalendarConnector calendarConnector = new GoogleCalendarConnector(SettingsConnector.ApplicationSettings.UserName, SettingsConnector.ApplicationSettings.Password, SettingsConnector.ApplicationSettings.CalendarUrl, SettingsConnector.ApplicationSettings.ApplicationName);

        EventDetailWindowViewModel viewModel = new EventDetailWindowViewModel(selectedEvent, calendarConnector, errorHelper);
        EventDetailWindow eventDetailWindow = new EventDetailWindow();
        eventDetailWindow.DataContext = viewModel;
        eventDetailWindow.ShowDialog();
      }
    }
  }
}
