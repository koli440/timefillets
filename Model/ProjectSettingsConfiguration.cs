using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeFillets.Model
{
  public class ProjectSettingsConfiguration
  {
    /// <summary>
    /// Regular expression for variable match
    /// </summary>
    public string VariablePattern { get; set; }
    
    /// <summary>
    /// Separator of key, value pair
    /// </summary>
    public string Separator { get; set; }

    /// <summary>
    /// Pattern of string with values
    /// </summary>
    public string Pattern { get; set; }

    /// <summary>
    /// Left bracket
    /// </summary>
    public string LeftBracket { get; set; }
    
    /// <summary>
    /// Right bracket
    /// </summary>
    public string RightBracket { get; set; }

    /// <summary>
    /// Key string of customer
    /// </summary>
    public string CustomerSign { get; set; }

    /// <summary>
    /// Key string of project
    /// </summary>
    public string ProjectSign { get; set; }

    /// <summary>
    /// Key string of task
    /// </summary>
    public string TaskSign { get; set; }
  }
}
