using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using RandomFileCopier.Dialogs;

namespace RandomFileCopier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Error(e.Exception.ToString());
            var dialogService = new DialogService();
            dialogService.ShowDialog(RandomFileCopier.Properties.Resources.SomethingWentWrong, RandomFileCopier.Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            
            e.Handled = true;
        }
    }
}
