using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TimeFillets.ViewModel
{
  public class ProgressWindowViewModel : ViewModelBase
  {
    private int _totalCount;
    private int _currentCount;
    private int _percentCount;
    private BackgroundWorker _worker;

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

    public ProgressWindowViewModel(BackgroundWorker worker)
    {
      _worker = worker;
      _worker.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);
    }

    void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      PercentCount = e.ProgressPercentage;
    }
  }
}
