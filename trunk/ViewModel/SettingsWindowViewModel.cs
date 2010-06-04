using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Connectors;

namespace TimeFillets.ViewModel
{
  public class SettingsWindowViewModel : ViewModelBase
  {
    public Uri CalendarUrl
    {
      get { return SettingsConnector.ApplicationSettings.CalendarUrl; }
      set { SettingsConnector.ApplicationSettings.CalendarUrl = value; }
    }

    public string UserName
    {
      get { return SettingsConnector.ApplicationSettings.UserName; }
      set { SettingsConnector.ApplicationSettings.UserName = value; }
    }

    public string Password
    {
      get { return SettingsConnector.ApplicationSettings.Password; }
      set { SettingsConnector.ApplicationSettings.Password = value; }
    }

    public bool ProxyInUse
    {
      get { return SettingsConnector.ApplicationSettings.ProxyInUse; }
      set { SettingsConnector.ApplicationSettings.ProxyInUse = value; }
    }

    public string ProxyDomain
    {
      get { return SettingsConnector.ApplicationSettings.ProxyDomain; }
      set { SettingsConnector.ApplicationSettings.ProxyDomain = value; }
    }
    
    public string ProxyUserName
    {
      get { return SettingsConnector.ApplicationSettings.ProxyUserName; }
      set { SettingsConnector.ApplicationSettings.ProxyUserName = value; }
    }
    
    public string ProxyPassword
    {
      get { return SettingsConnector.ApplicationSettings.ProxyPassword; }
      set { SettingsConnector.ApplicationSettings.ProxyPassword = value; }
    }
  }
}
