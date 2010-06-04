using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace TimeFillets.ViewModel
{
  public class CommandViewModel : ViewModelBase
  {
    public ICommand Command { get; private set; }

    public CommandViewModel(string displayName, ICommand command)
    {
      if (command == null)
        throw new ArgumentNullException("command");

      base.DisplayName = displayName;
      this.Command = command;
    }
  }
}
