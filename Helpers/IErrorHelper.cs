using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeFillets.Helpers
{
  public interface IErrorHelper
  {
    void ShowError(string message);
    void ShowError(Exception e);
  }
}
