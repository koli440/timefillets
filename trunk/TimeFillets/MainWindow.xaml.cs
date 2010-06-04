using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TimeFillets.Model;
using TimeFillets.Connectors;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;
using TimeFillets.ViewModel;
using System.Configuration;

namespace TimeFillets.MainApplication
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    protected GoogleCalendarConnector connector;

    public MainWindow()
    {
      InitializeComponent();
      ItemsListView.MouseDoubleClick += new MouseButtonEventHandler(ItemsListView_MouseDoubleClick);
    }

    void ItemsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.ItemDetailCommand.Command.Execute((CalendarItem)ItemsListView.SelectedItem);
    }

    private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      this.Close();
    }

    private void FilePathBrowseButton_Click(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
      if (((MainWindowViewModel)DataContext).From != null && ((MainWindowViewModel)DataContext).To != null)
        dialog.FileName = string.Format("TimeSheet {0}-{1}", ((MainWindowViewModel)DataContext).From.Value.ToString("yyyy-MM-dd"), ((MainWindowViewModel)DataContext).To.Value.ToString("yyyy-MM-dd"));
      else
        dialog.FileName = "TimeSheet";
      dialog.DefaultExt = (string)ExportFormatComboBox.SelectedValue;
      dialog.Filter = "Microsoft Excel (.xlsx)|*.xlsx|All Files|*.*";

      Nullable<bool> result = dialog.ShowDialog();

      if (result == true)
      {
        ((MainWindowViewModel)DataContext).ExportPath = dialog.FileName;
      }
    }
  }
}
