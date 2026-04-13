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
using RandomFileCopier.Models.Selection;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    internal class AudioCopyViewModel 
        : FileCopyViewModel<AudioSourceDestinationModel, AudioSelectionModel, CopyRepresenter>
    {
        private readonly IRandomAudioFileSelector _randomAudioFileSelector;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public AudioCopyViewModel(IFileSearcher fileSearcher,  IDispatcherWrapper dispatcher, IRandomAudioFileSelector randomFileSelector, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper, IConfigurationHelper configurationHelper)
            : base(fileSearcher ?? new FileSearcher(), dispatcher ?? new DispatcherWrapper(), serializationHelper ?? new SerializationHelper(), dialogService ?? new DialogService(), openerHelper ?? new OpenerHelper(), configurationHelper ?? new ConfigurationHelper())
        {
            _randomAudioFileSelector = randomFileSelector ?? new RandomAudioFileSelector();
            SelectionModel = new AudioSelectionModel(0, 350);
            var settings = ConfigurationHelper.GetExtensions(ExtensionsAppsettingKey.AudioExtensions);
            AudioExtensions = new ObservableCollection<string>(settings.Select(x => x.Extension));
            Model = new AudioSourceDestinationModel(settings.Where(x => x.DefaultSelected).Select(x => x.Extension));
            
        }

        public AudioCopyViewModel() 
            : this(null, null,null, null, null, null, null)
        {
        }

        protected override CopyRepresenter CreateFileRepresenter(FileInfo fileInfo)
        {
            var representer = new CopyRepresenter(fileInfo.FullName, fileInfo.Name, fileInfo.Length);
            representer.DurationInSeconds = MediaDurationReader.GetDurationInSeconds(fileInfo.FullName);
            return representer;
        }


        protected override Task SelectRandomFilesAsync(List<CopyRepresenter> filesList, List<MovedOrCopiedFile> copiedFileList, CancellationToken token )
        {
            var selectionSettings = new RandomAudioSelectionSettings()
            {
                Files = filesList.ToList(),
                CopiedFileList = copiedFileList,
                AvoidDuplicates = SelectionModel.AvoidDuplicates,
                FileSizeSelectionSettings = new FileSizeSelectionSettings()
                {
                    MaximumFileSize = SelectionModel.MaximumFileSizeInBytes,
                    MaximumSize = SelectionModel.SelectedSizeInBytes,
                    MinimumFileSize = SelectionModel.MinimumFileSizeInBytes
                },
                DurationSelectionSettings = new DurationSelectionSettings()
                {
                    MaximumDuration = SelectionModel.MaximumDurationInSeconds,
                    MinimumDuration = SelectionModel.MinimumDurationInSeconds,
                    IncludeFilesWithoutDuration = SelectionModel.IncludeFilesWithoutDuration
                }
            };

            return _randomAudioFileSelector.SelectMaximumAmountOfRandomFilesAsync(selectionSettings, token);
        }

        public ObservableCollection<string> AudioExtensions { get; set; }
    }
}
