using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Dialogs;
using RandomFileCopier.Helpers;
using RandomFileCopier.Logic;
using RandomFileCopier.Logic.Copier;
using RandomFileCopier.Logic.Helper;
using RandomFileCopier.Models;
using RandomFileCopier.Models.Base;
using RandomFileCopier.Models.Selection.Base;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    class FolderCopyViewModel
        : CopyViewModel<SourceDestinationModel<CopyRepresenter>,SelectionModel , CopyRepresenter>
    {
        private readonly IFolderSearcher _folderSearcher;
        private readonly IRandomFolderSelector _folderSelector;
        private readonly IFolderCopier _folderCopier;

        public FolderCopyViewModel(IDispatcherWrapper dispatcher, IFolderSearcher folderSearcher, IRandomFolderSelector folderSelector, IFolderCopier folderCopier, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper) 
            : base(dispatcher ?? new DispatcherWrapper(), serializationHelper ?? new SerializationHelper(), dialogService ?? new DialogService(), openerHelper?? new OpenerHelper())
        {
            _folderSearcher = folderSearcher ?? new FolderSearcher();
            _folderSelector = folderSelector ?? new RandomFolderSelector();
            _folderCopier = folderCopier ?? new FolderCopier();
            SelectionModel = new SelectionModel(0, 100, UnitSize.GB);
            Model = new SourceDestinationModel<CopyRepresenter>();
        }

        public FolderCopyViewModel() 
            : this(null, null, null, null, null, null, null)
        {

        }

        protected async override Task SpecificSearchAsync(string path, CancellationToken token)
        {
            foreach (var item in _folderSearcher.SearchFolders(path))
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    var newFile = new CopyRepresenter(item.FullName, item.Name, _folderSearcher.CalculateDirSize(item));
                    Dispatcher.Invoke(() => Model.Items.Add(newFile));
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine(path);
                }
            }
            var copiedFileList = GetFileListOrNullIfNotApplicable();
            await SelectRandomFilesAsync(Model.Items, copiedFileList, token);
        }

        protected override Task SelectRandomFilesAsync(IEnumerable<CopyRepresenter> copyRepresenterList, IEnumerable<CopiedFile> copiedFileList, CancellationToken token)
        {
           return _folderSelector.SelectMaximumAmountOfRandomFoldersAsync(copyRepresenterList, SelectionModel.MinimumFileSizeInBytes, SelectionModel.MaximumFileSizeInBytes ,SelectionModel.SelectedSizeInBytes, copiedFileList, token);
        }

        protected override Task<IListWithErrorDictionary<CopiedFile>> CopySpecificAsync(CancellationToken token)
        {
            return Task.Run<IListWithErrorDictionary<CopiedFile>>(async () => {
                var copiedList = new ListWithErrorDictionary<CopiedFile>();
                var selectedItems = Model.Items.Where(x => x.IsSelected).ToList();
                Progress = 0;
                MaxProgressBarValue = selectedItems.Count;
                foreach (var file in selectedItems)
                {
                    token.ThrowIfCancellationRequested();
                    Progress++;
                    await _folderCopier.DirectoryCopyAsync(file.Path, Model.DestinationPath, token);
                    copiedList.Add(new CopiedFile(file.Path, file.Size, DateTime.Now));
                }
                return copiedList;
            });
        }
    }
}
