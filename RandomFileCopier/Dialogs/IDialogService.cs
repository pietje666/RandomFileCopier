using System.Windows;

namespace RandomFileCopier.Dialogs
{
    interface IDialogService
    {
        void ShowDialog(string messageText, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage);
    }
}