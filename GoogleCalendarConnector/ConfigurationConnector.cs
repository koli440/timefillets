using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using TimeFillets.Model;

namespace TimeFillets.Connectors
{
  public static class ConfigurationConnector
  {
    private static ProjectSettingsConfiguration _projectSettings;

    public static ProjectSettingsConfiguration ProjectSettings
    {
      get
      {
        if (_projectSettings == null)
          _projectSettings = (ProjectSettingsConfiguration)ConfigurationManager.GetSection("projectSettings");
        return _projectSettings;
      }
    }
  }
}
