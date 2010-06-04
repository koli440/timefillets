using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeFillets.Helpers;
using System.Diagnostics;

namespace TimeFillets.Tests
{
  class EmptyErrorHelper : IErrorHelper
  {
    #region IErrorHelper Members

    public void ShowError(string message)
    {
      Trace.WriteLine("ShowError(string message) triggered");
    }

    public void ShowError(Exception e)
    {
      Trace.WriteLine("ShowError(Exception e) triggered");
    }

    #endregion
  }
}
