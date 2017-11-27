using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Helper
{
    internal class OpenerHelper : IOpenerHelper
    {

        public string OpenDestinationFileDialog(bool showNewFolderButton = false)
        {
            string path = null;
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = showNewFolderButton;
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }
            return path;
        }

        public void OpenFolderHandler(CopyRepresenter selectedItem)
        {
            if (Directory.Exists(selectedItem.Path))
            {
                Process.Start("explorer.exe", selectedItem.Path);
            }
            else
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(selectedItem.Path));
            }
        }
    }
}
