using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Helper
{
    internal interface IOpenerHelper
    {
        string OpenDestinationFileDialog(bool showNewFolderButton = false);
        void OpenFolderHandler(CopyRepresenter selectedItem);
    }
}