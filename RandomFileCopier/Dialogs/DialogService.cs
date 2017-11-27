using System;
using System.Linq;
using System.Windows;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace RandomFileCopier.Dialogs
{
    internal class DialogService : IDialogService
    {

        public void ShowDialog(string messageText, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            var style = new Style(typeof(MessageBox));
            style.Setters.Add(new Setter(MessageBox.WindowBackgroundProperty, System.Windows.Media.Brushes.White));
            style.Setters.Add(new Setter(MessageBox.BorderBrushProperty, System.Windows.Media.Brushes.LightGray));

            MessageBox.Show(Application.Current.MainWindow, messageText, caption, messageBoxButton, messageBoxImage, style);
        }

        internal void ShowDialog(object somethingWentWrong, object error1, MessageBoxButton oK, MessageBoxImage error2)
        {
            throw new NotImplementedException();
        }
    }
}
