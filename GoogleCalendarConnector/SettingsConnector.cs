using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using TimeFillets.Model;

namespace TimeFillets.Connectors
{
  /// <summary>
  /// Provides settings to application
  /// </summary>
  public static class SettingsConnector
  {
    private static ApplicationSettings applicationSettings;
    private static ProjectsDefinitions projectsDefinitions;

    /// <summary>
    /// Application settings stored in local registry store
    /// </summary>
    public static ApplicationSettings ApplicationSettings
    {
      get
      {
        if (applicationSettings == null)
          applicationSettings = new ApplicationSettings();
        return applicationSettings;
      }
    }

    /// <summary>
    /// Project definitions stored in xml files
    /// </summary>
    public static ProjectsDefinitions ProjectsDefinitions
    {
      get
      {
        if (projectsDefinitions == null)
        {
          ProjectDefinitionsConnector connector = new ProjectDefinitionsConnector();
          projectsDefinitions = connector.GetDefinitions();
        }
        return projectsDefinitions;
      }
    }

    /// <summary>
    /// Refreshes project definitions
    /// </summary>
    /// <returns></returns>
    public static ProjectsDefinitions RefreshProjectsDefinistions()
    {
      ProjectDefinitionsConnector connector = new ProjectDefinitionsConnector();
      projectsDefinitions = connector.GetDefinitions();
      return projectsDefinitions;
    }
  }
}
