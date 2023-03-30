using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Dialogs;
using RandomFileCopier.Helpers;
using RandomFileCopier.Logic;
using RandomFileCopier.Logic.Helper;
using RandomFileCopier.Logic.Selectors;
using RandomFileCopier.Models;
using RandomFileCopier.Models.Selection.Base;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    internal class PictureCopyViewModel
        : FileCopyViewModel<PictureSourceDestinationModel, SelectionModel, CopyRepresenter>
    {
        private readonly IRandomPictureFileSelector _randomPictureFileSelector;

        public PictureCopyViewModel(IFileSearcher fileSearcher, IDispatcherWrapper dispatcher, IRandomPictureFileSelector randomFileSelector, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper, IConfigurationHelper configurationHelper)
            : base(fileSearcher ?? new FileSearcher(), dispatcher ?? new DispatcherWrapper(), serializationHelper ?? new SerializationHelper(), dialogService ?? new DialogService(), openerHelper ?? new OpenerHelper(), configurationHelper ?? new ConfigurationHelper())
        {
            _randomPictureFileSelector = randomFileSelector ?? new RandomPictureFileSelector();
            SelectionModel = new SelectionModel(0, 350, UnitSize.MB);
            var settings = ConfigurationHelper.GetExtensions(ExtensionsAppsettingKey.PictureExtensions);
            PictureExtensions = new ObservableCollection<string>(settings.Select(x => x.Extension));
            Model = new PictureSourceDestinationModel(settings.Where(x => x.DefaultSelected).Select(x => x.Extension));

        }

        public PictureCopyViewModel()
            : this(null, null, null, null, null, null, null)
        {
        }

        protected override CopyRepresenter CreateFileRepresenter(FileInfo fileInfo)
        {
            return new CopyRepresenter(fileInfo.FullName, fileInfo.Name, fileInfo.Length);
        }

        protected override Task SelectRandomFilesAsync(IEnumerable<CopyRepresenter> filesList, IEnumerable<MovedOrCopiedFile> copiedFileList, CancellationToken token)
        {
            return _randomPictureFileSelector.SelectMaximumAmountOfRandomFilesAsync(filesList, SelectionModel.MinimumFileSizeInBytes, SelectionModel.MaximumFileSizeInBytes, SelectionModel.SelectedSizeInBytes, copiedFileList, SelectionModel.AvoidDuplicates, token);
        }

        public ObservableCollection<string> PictureExtensions { get; set; }
    }
}
