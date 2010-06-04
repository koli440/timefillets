﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.GData.Calendar;
using Google.GData.Extensions;
using TimeFillets.Model;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;

namespace TimeFillets.Connectors
{
  /// <summary>
  /// Provides functionality to operate with Google calendar
  /// </summary>
  public class GoogleCalendarConnector : ConnectorBase, ICalendarConnector
  {

    private CalendarService service;
    private string appName;

    #region fields
    private string _userName;
    private string _password;
    private Uri _calendarUrl;
    private bool _isConnected;
    #endregion

    #region properties
    /// <summary>
    /// Username for calendar access
    /// </summary>
    public string UserName
    {
      get { return _userName; }
      private set
      {
        if (value == _userName)
          return;

        _userName = value;
        OnPropertyChanged("UserName");
      }
    }

    /// <summary>
    /// Password for calendar access
    /// </summary>
    public string Password
    {
      get { return _password; }
      private set
      {
        if (value == _password)
          return;

        _password = value;
        OnPropertyChanged("Password");
      }
    }

    /// <summary>
    /// Calendar Url
    /// </summary>
    public Uri CalendarUrl
    {
      get { return _calendarUrl; }
      private set
      {
        if (value == _calendarUrl)
          return;

        _calendarUrl = value;
        OnPropertyChanged("CalendarUrl");
      }
    }

    /// <summary>
    /// True if connector is connected to google calendar
    /// </summary>
    public bool IsConnected
    {
      get { return _isConnected; }
      private set
      {
        if (value == _isConnected)
          return;

        _isConnected = value;
        OnPropertyChanged("IsConnected");
      }
    }

    #endregion
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userName">Username for calendar connection</param>
    /// <param name="password">Password for calendar connection</param>
    /// <param name="calendarUrl">Url of calendar</param>
    /// <param name="applicationName">Name of application</param>
    public GoogleCalendarConnector(string userName, string password, Uri calendarUrl, string applicationName)
    {
      this.UserName = userName;
      this.Password = password;
      this.CalendarUrl = calendarUrl;
      this.appName = applicationName;
      this.service = new CalendarService(appName);
      this.service.setUserCredentials(userName, password);
      if (SettingsConnector.ApplicationSettings.ProxyInUse)
      {
        this.SetUpProxy((GDataRequestFactory)this.service.RequestFactory, calendarUrl);
      }
      this.IsConnected = false;
    }

    /// <summary>
    /// Returns all calendar items
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CalendarItem> GetCalendarItems()
    {
      return GetCalendarItems(DateTime.MinValue, DateTime.MaxValue);
    }

    /// <summary>
    /// Get calendar items in given time range
    /// </summary>
    /// <param name="from">Begining of time range</param>
    /// <param name="to">End of time range</param>
    /// <returns></returns>
    public IEnumerable<CalendarItem> GetCalendarItems(DateTime from, DateTime to)
    {
      List<CalendarItem> calendarItems = new List<CalendarItem>();

      EventQuery query = new EventQuery();
      query.Uri = CalendarUrl;
      query.StartDate = from;
      query.EndDate = to;
      query.NumberToRetrieve = 1000;

      try
      {
        EventFeed calendarFeed = GetEventFeed(query);
        this.IsConnected = true;

        if (calendarFeed != null)
        {
          ProjectDefinitionsConnector definitionsConnector = new ProjectDefinitionsConnector();
          var definitions = definitionsConnector.GetDefinitions();
          foreach (EventEntry entry in calendarFeed.Entries)
          {
            Customer customer = null;
            Project project = null;
            Task task = null;

            // customer
            Regex regex = ProjectSettings.CustomerRegex;
            Match match = regex.Match(entry.Content.Content);
            string name = string.Empty;
            if (match.Success)
              name = ProjectSettings.GetCustomerName(match.Value);
            var retCustomer = definitions.Customers.Where(c => c.Name == name).FirstOrDefault();
            if (retCustomer != null)
              customer = retCustomer;
            else
              customer = new Customer() { Name = name };

            // project
            regex = ProjectSettings.ProjectRegex;
            match = regex.Match(entry.Content.Content);
            name = string.Empty;
            if (match.Success)
              name = ProjectSettings.GetProjectName(match.Value);
            if (customer.Projects.Count > 0)
            {
              var retProject = customer.Projects.Where(p => p.Name == name).FirstOrDefault();
              if (retProject != null)
                project = retProject;
              else
                project = new Project() { Name = name };
            }
            else
              project = new Project() { Name = name };

            //task
            regex = ProjectSettings.TaskRegex;
            match = regex.Match(entry.Content.Content);
            name = string.Empty;
            if (match.Success)
              name = ProjectSettings.GetTaskName(match.Value);
            if (project.Tasks.Count > 0)
            {
              var retTask = project.Tasks.Where(item => item.Name == name).FirstOrDefault();
              if (retTask != null)
                task = retTask;
              else
                task = new Task() { Name = name };
            }
            else
              task = new Task() { Name = name };

            CalendarItem calendarItem = new CalendarItem(customer, project, task);
            calendarItem.Title = entry.Title.Text;
            calendarItem.Description = entry.Content.Content;
            calendarItem.Updated = entry.Updated;
            calendarItem.GoogleEventId = entry.EventId;
            if (entry.Times.Count > 0)
            {
              calendarItem.StartDate = entry.Times.First().StartTime;
              calendarItem.EndDate = entry.Times.First().EndTime;
            }
            calendarItem.IsRepeating = entry.Times.Count() == 0;
            calendarItem.Location = entry.Locations.First().ValueString;

            calendarItems.Add(calendarItem);
          }
        }
      }
      catch (Exception e)
      {
        Trace.WriteLine(e.Message);
        this.IsConnected = false;
        throw;
      }

      return calendarItems.OrderBy(itm => itm.StartDate).ToList();
    }

    /// <summary>
    /// Gets calendar items for given project
    /// </summary>
    /// <param name="project">Project for witch items should be returned</param>
    /// <returns></returns>
    public IEnumerable<CalendarItem> GetCalendarItems(Project project)
    {
      List<CalendarItem> ret = GetCalendarItems().Where(itm => itm.Description.Contains(ProjectSettings.GetProjectString(project.Name))).ToList();
      return ret;
    }

    /// <summary>
    /// Gets calendar items for given customer
    /// </summary>
    /// <param name="customer">Customer for witch items should be returned</param>
    /// <returns></returns>
    public IEnumerable<CalendarItem> GetCalendarItems(Customer customer)
    {
      List<CalendarItem> ret = GetCalendarItems().Where(itm => itm.Description.Contains(ProjectSettings.GetCustomerString(customer.Name))).ToList();
      return ret;
    }

    /// <summary>
    /// Gets calendar items for given task
    /// </summary>
    /// <param name="task">Task for witch should be items returned</param>
    /// <returns></returns>
    public IEnumerable<CalendarItem> GetCalendarItems(Task task)
    {
      List<CalendarItem> ret = GetCalendarItems().Where(itm => itm.Description.Contains(ProjectSettings.GetTaskString(task.Name))).ToList();
      return ret;
    }

    /// <summary>
    /// Saves calendar item back to store
    /// </summary>
    /// <param name="item">item that should be saved</param>
    public void EditCalendarItem(CalendarItem item)
    {
      EventQuery query = new EventQuery();
      query.Uri = CalendarUrl;
      query.NumberToRetrieve = 1000;

      EventFeed feed = GetEventFeed(query);
      if (feed != null)
      {
        EventEntry entry = feed.Entries.Where(itm => (itm as EventEntry).EventId == item.GoogleEventId).First() as EventEntry;
        
        entry.Title.Text = item.Title;
        entry.Content.Content = item.Description;
        entry.Updated = DateTime.Now;
        if (entry.Times.Count > 0)
        {
          entry.Times.First().StartTime = item.StartDate;
          entry.Times.First().EndTime = item.EndDate;
        }
        else
        {
          entry.Times.Add(new When(item.StartDate, item.EndDate, false));
        }

        if (entry.Locations.Count > 0)
          entry.Locations.First().ValueString = item.Location;
        else
        {
          entry.Locations.Add(new Where() { ValueString = item.Location });
        }

        

        entry.Update();
      }
    }

    private EventFeed GetEventFeed(EventQuery query)
    {
      return service.Query(query) as EventFeed;
    }

    private void SetUpProxy(GDataRequestFactory requestFactory, Uri calendarUrl)
    {
      IWebProxy iProxy = WebRequest.DefaultWebProxy;
      WebProxy myProxy = new WebProxy(iProxy.GetProxy(calendarUrl));
      NetworkCredential credentials = new NetworkCredential(SettingsConnector.ApplicationSettings.ProxyUserName, SettingsConnector.ApplicationSettings.ProxyPassword, SettingsConnector.ApplicationSettings.ProxyDomain);
      myProxy.Credentials = credentials;
      myProxy.UseDefaultCredentials = true;
      requestFactory.Proxy = myProxy;
    }
  }
}