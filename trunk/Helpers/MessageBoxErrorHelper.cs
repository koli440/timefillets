using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TimeFillets.Helpers
{
  public class MessageBoxErrorHelper : IErrorHelper
  {
    #region IErrorHelper Members

    public void ShowError(string errorMessage)
    {
      MessageBox.Show(errorMessage);
    }
    
    public void ShowError(Exception e)
    {
      StringBuilder messageBuilder = new StringBuilder();

      messageBuilder.AppendFormat("Exception: {0}\n", e.Message);
      if (e.InnerException != null)
        messageBuilder.AppendFormat("Inner Exception: {0}\n", e.InnerException.Message);
      
      #if (DEBUG)
      messageBuilder.AppendFormat("Stack trace: {0}\n", e.StackTrace);
      #endif

      MessageBox.Show(messageBuilder.ToString());
    }

    #endregion
  }
}
