using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Collections;

namespace TimeFillets.Model
{
  public class ProjectsDefinitions
  {
    public List<Customer> Customers { get; set; }

    public ProjectsDefinitions(List<Customer> customers)
    {
      Customers = customers;
    
    }
  }
}
