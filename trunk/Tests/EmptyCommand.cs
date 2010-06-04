using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace TimeFillets.Tests
{
  class EmptyCommand : ICommand
  {
    #region ICommand Members

    public void Execute(object parameter)
    {
      Trace.WriteLine("Execute triggered");
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;

    #endregion
  }
}
