using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TimeFillets.Model
{
  /// <summary>
  /// Project entity
  /// </summary>
  [Serializable]
  public class Project
  {
    /// <summary>
    /// Projects name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Customer which owns this project
    /// </summary>
    public Customer Owner { get; set; }
    
    /// <summary>
    /// Tasks of this project
    /// </summary>
    public List<Task> Tasks { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Project()
    {
      Tasks = new List<Task>();
    }
  }
}
