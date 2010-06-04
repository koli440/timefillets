using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeFillets.Model
{
  public class Task
  {
    public string Name { get; set; }
    public Project Owner { get; set; }
  }
}
