using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeFillets.Model
{
  /// <summary>
  /// Customer entity
  /// </summary>
  public class Customer
  {
    /// <summary>
    /// Customer name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Projects of this customer
    /// </summary>
    public List<Project> Projects { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Customer()
    {
      Projects = new List<Project>();
    }
  }
}
