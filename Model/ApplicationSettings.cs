using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TimeFillets.Model
{
  /// <summary>
  /// Application settings stored in local registry store
  /// </summary>
  public class ApplicationSettings
  {
    private const string manufacturerKey = "SoulFilets";
    private const string applicationNameKey = "GoogleCalendarTimeSheet";
    private const string calendarUrlKey = "CalendarUrl";
    private const string userNameKey = "UserName";
    private const string passwordKey = "Password";
    private const string proxyInUseKey = "UseProxy";
    private const string proxyDomainKey = "ProxyDomain";
    private const string proxyUserNameKey = "ProxyUserName";
    private const string proxyPasswordKey = "ProxyPasswor";

    private RegistryKey applicationRootKey;

    /// <summary>
    /// Readonly name of application
    /// </summary>
    public readonly string ApplicationName = "GoogleCalendarTimeSheet";

    /// <summary>
    /// Default constructor
    /// </summary>
    public ApplicationSettings()
    {
      RegistryKey manKey = Registry.CurrentUser.CreateSubKey(manufacturerKey);
      applicationRootKey = manKey.CreateSubKey(applicationNameKey);
    }

    /// <summary>
    /// Calendars url
    /// </summary>
    public Uri CalendarUrl
    {
      get
      {
        Uri url;
        try
        {
          url = new Uri((string)applicationRootKey.GetValue(calendarUrlKey, string.Empty));
        }
        catch
        {
          url = new Uri("http://www.google.com/calendar/feeds/default/private/full");
        }
        return url;
      }
      set
      {
        applicationRootKey.SetValue(calendarUrlKey, value.OriginalString);
      }
    }

    /// <summary>
    /// Username for calendar connection
    /// </summary>
    public string UserName
    {
      get
      {
        return (string)applicationRootKey.GetValue(userNameKey, string.Empty);
      }
      set
      {
        applicationRootKey.SetValue(userNameKey, value);
      }
    }

    /// <summary>
    /// Password for calendar connection
    /// </summary>
    public string Password
    {
      get
      {
        return (string)applicationRootKey.GetValue(passwordKey, string.Empty);
      }
      set
      {
        applicationRootKey.SetValue(passwordKey, value);
      }
    }

    /// <summary>
    /// Username for proxy
    /// </summary>
    public string ProxyUserName
    {
      get
      {
        return (string)applicationRootKey.GetValue(proxyUserNameKey, string.Empty);
      }
      set
      {
        applicationRootKey.SetValue(proxyUserNameKey, value);
      }
    }

    /// <summary>
    /// Password for proxy
    /// </summary>
    public string ProxyPassword
    {
      get
      {
        return (string)applicationRootKey.GetValue(proxyPasswordKey, string.Empty);
      }
      set
      {
        applicationRootKey.SetValue(proxyPasswordKey, value);
      }
    }

    /// <summary>
    /// Domain for proxy
    /// </summary>
    public string ProxyDomain
    {
      get
      {
        return (string)applicationRootKey.GetValue(proxyDomainKey, string.Empty);
      }
      set
      {
        applicationRootKey.SetValue(proxyDomainKey, value);
      }
    }

    /// <summary>
    /// Specifies if proxy should be used for connection
    /// </summary>
    public bool ProxyInUse
    {
      get
      {
        return bool.Parse((string)applicationRootKey.GetValue(proxyInUseKey, "false"));
      }
      set
      {
        applicationRootKey.SetValue(proxyInUseKey, value);
      }
    }

    /// <summary>
    /// If is configuration valid returns true
    /// </summary>
    public bool IsConfigurationValid
    {
      get
      {
        try
        {
          if (string.IsNullOrWhiteSpace(CalendarUrl.OriginalString))
            return true;
          if (string.IsNullOrWhiteSpace(UserName))
            return false;
          if (string.IsNullOrWhiteSpace(Password))
            return false;

          if (ProxyInUse)
          {
            if (string.IsNullOrWhiteSpace(ProxyUserName))
              return false;
            if (string.IsNullOrWhiteSpace(ProxyPassword))
              return false;
          }
          return true;
        }
        catch (Exception)
        {
          return false;
        }
      }
    }
  }
}
