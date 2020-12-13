using System;
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
using RandomFileCopier.Models;
using RandomFileCopier.Models.Selection.Base;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    internal class AudioCopyViewModel 
        : FileCopyViewModel<AudioSourceDestinationModel, SelectionModel, CopyRepresenter>
    {
        private readonly IRandomAudioFileSelector _randomAudioFileSelector;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public AudioCopyViewModel(IFileSearcher fileSearcher,  IDispatcherWrapper dispatcher, IRandomAudioFileSelector randomFileSelector, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper, IConfigurationHelper configurationHelper)
            : base(fileSearcher ?? new FileSearcher(), dispatcher ?? new DispatcherWrapper(), serializationHelper ?? new SerializationHelper(), dialogService ?? new DialogService(), openerHelper ?? new OpenerHelper(), configurationHelper ?? new ConfigurationHelper())
        {
            _randomAudioFileSelector = randomFileSelector ?? new RandomAudioFileSelector();
            SelectionModel = new SelectionModel(0, 350, UnitSize.MB);
            var settings = ConfigurationHelper.GetExtensions(ExtensionsAppsettingKey.AudioExtensions);
            Extensions = new ObservableCollection<string>(settings.Select(x => x.Extension));
            Model = new AudioSourceDestinationModel(settings.Where(x => x.DefaultSelected).Select(x => x.Extension));
            
        }

        public AudioCopyViewModel() 
            : this(null, null,null, null, null, null, null)
        {
        }

        protected override CopyRepresenter CreateFileRepresenter(FileInfo fileInfo)
        {
            return new CopyRepresenter(fileInfo.FullName, fileInfo.Name, fileInfo.Length);
        }

        protected override Task SelectRandomFilesAsync(IEnumerable<CopyRepresenter> filesList, IEnumerable<CopiedFile> copiedFileList, CancellationToken token )
        {
           return _randomAudioFileSelector.SelectMaximumAmountOfRandomFilesAsync(filesList, SelectionModel.MinimumFileSizeInBytes ,SelectionModel.MaximumFileSizeInBytes, SelectionModel.SelectedSizeInBytes,copiedFileList, token);
        }
    }
}
