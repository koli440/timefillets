using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TimeFillets.Model
{
  public static class ProjectSettings
  {
    /// <summary>
    /// Gets regular expression for selecting a customer
    /// </summary>
    public static Regex CustomerRegex
    {
      get
      {
        Regex ret = new Regex(string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.CustomerSign, ConfigurationConnector.ProjectSettings.VariablePattern, ConfigurationConnector.ProjectSettings.Separator));
        return ret;
      }
    }

    /// <summary>
    /// Gets regular expression for selecting a project
    /// </summary>
    public static Regex ProjectRegex
    {
      get
      {
        Regex ret = new Regex(string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.ProjectSign, ConfigurationConnector.ProjectSettings.VariablePattern, ConfigurationConnector.ProjectSettings.Separator));
        return ret;
      }
    }

    /// <summary>
    /// Gets regular expression for selecting a task
    /// </summary>
    public static Regex TaskRegex
    {
      get
      {
        Regex ret = new Regex(string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.TaskSign, ConfigurationConnector.ProjectSettings.VariablePattern, ConfigurationConnector.ProjectSettings.Separator));
        return ret;
      }
    }

    /// <summary>
    /// Gets project string
    /// </summary>
    /// <param name="projectName">Name of the project</param>
    /// <returns></returns>
    public static string GetProjectString(string projectName)
    {
      return string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.ProjectSign, projectName, ConfigurationConnector.ProjectSettings.Separator);
    }

    /// <summary>
    /// Gets customer string
    /// </summary>
    /// <param name="customerName">Name of customer</param>
    /// <returns></returns>
    public static string GetCustomerString(string customerName)
    {
      return string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.CustomerSign, customerName, ConfigurationConnector.ProjectSettings.Separator);
    }

    /// <summary>
    /// Gets task string
    /// </summary>
    /// <param name="taskName">Task name</param>
    /// <returns></returns>
    public static string GetTaskString(string taskName)
    {
      return string.Format(ConfigurationConnector.ProjectSettings.Pattern, ConfigurationConnector.ProjectSettings.LeftBracket, ConfigurationConnector.ProjectSettings.RightBracket, ConfigurationConnector.ProjectSettings.TaskSign, taskName, ConfigurationConnector.ProjectSettings.Separator);
    }

    /// <summary>
    /// Returns name of project from string
    /// </summary>
    /// <param name="projectString">Project string from regular expression</param>
    /// <returns></returns>
    public static string GetProjectName(string projectString)
    {
      return projectString.Replace(ConfigurationConnector.ProjectSettings.LeftBracket, "").Replace(ConfigurationConnector.ProjectSettings.RightBracket, "").Replace(ConfigurationConnector.ProjectSettings.ProjectSign, "").Replace(ConfigurationConnector.ProjectSettings.Separator, "");
    }

    /// <summary>
    /// Gets customer name from string
    /// </summary>
    /// <param name="customerString">Customer string from regular expression</param>
    /// <returns></returns>
    public static string GetCustomerName(string customerString)
    {
      return customerString.Replace(ConfigurationConnector.ProjectSettings.LeftBracket, "").Replace(ConfigurationConnector.ProjectSettings.RightBracket, "").Replace(ConfigurationConnector.ProjectSettings.CustomerSign, "").Replace(ConfigurationConnector.ProjectSettings.Separator, "");
    }

    /// <summary>
    /// Gets task name from string
    /// </summary>
    /// <param name="customerString">Task string from regular expression</param>
    /// <returns></returns>
    public static string GetTaskName(string taskString)
    {
      return taskString.Replace(ConfigurationConnector.ProjectSettings.LeftBracket, "").Replace(ConfigurationConnector.ProjectSettings.RightBracket, "").Replace(ConfigurationConnector.ProjectSettings.TaskSign, "").Replace(ConfigurationConnector.ProjectSettings.Separator, "");
    }
  }
}
