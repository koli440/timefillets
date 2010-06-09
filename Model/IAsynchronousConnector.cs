using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TimeFillets.Model
{
  public interface IAsynchronousConnector
  {
    BackgroundWorker Worker { get; set; }
    void ReportWork(int percentCompleted);
  }
}
