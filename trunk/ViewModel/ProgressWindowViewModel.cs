using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace TimeFillets.ViewModel
{
  public class ProgressWindowViewModel : ViewModelBase
  {
    private int _totalCount;
    private int _currentCount = 100;
    private int _percentCount = 0;
    private BackgroundWorker _worker;
    private CommandViewModel _closeCommand;


    public CommandViewModel CloseCommand
    {
      get { return _closeCommand; }
      private set { _closeCommand = value; }
    }

    /// <summary>
    /// Total items count
    /// </summary>
    public int TotalCount
    {
      get { return _totalCount; }
      set
      {
        if (value == _totalCount)
          return;

        _totalCount = value;
        base.OnPropertyChanged("TotalCount");
      }
    }

    /// <summary>
    /// Current progress count
    /// </summary>
    public int CurrentCount
    {
      get { return _currentCount; }
      set
      {
        if (value == _currentCount)
          return;

        _currentCount = value;
        base.OnPropertyChanged("CurrentCount");
      }
    }

    public int PercentCount
    {
      get { return _percentCount; }
      set
      {
        if (value == _percentCount)
          return;

        _percentCount = value;
        base.OnPropertyChanged("PercentCount");
      }
    }

    public ProgressWindowViewModel(BackgroundWorker worker, CommandViewModel closeCommand)
    {
      if (closeCommand == null)
        throw new ArgumentNullException("closeCommand", "Close command cannot be null");
      CloseCommand = closeCommand;

      _worker = worker;
      _worker.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);
      _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
      _worker.RunWorkerAsync();
    }

    void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      CloseCommand.Command.Execute(new Object());
    }

    void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.PercentCount = e.ProgressPercentage;
    }
  }
}
