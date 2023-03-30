using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    class VideoCopyViewModel
        : FileCopyViewModel<VideoSourceDestinationModel, VideoSelectionModel , VideoFileRepresenter>
    {
        private readonly IVideoFileRepresenterFactory _fileRepresenterFactory;
        private readonly IRandomVideoFileSelector _randomFileSelector;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public VideoCopyViewModel(IFileSearcher fileSearcher, IVideoFileRepresenterFactory videoFileRepresenterFactory, IDispatcherWrapper dispatcher, IRandomVideoFileSelector randomFileSelector, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper, IConfigurationHelper configurationHelper)
            : base( fileSearcher ?? new FileSearcher(), dispatcher ?? new DispatcherWrapper(), serializationHelper?? new SerializationHelper(), dialogService ?? new DialogService(), openerHelper ?? new OpenerHelper(), configurationHelper ?? new ConfigurationHelper())
        {
            _fileRepresenterFactory = videoFileRepresenterFactory  ?? new VideoFileRepresenterFactory();
            _randomFileSelector = randomFileSelector ?? new RandomVideoFileSelector();
            SelectionModel = new VideoSelectionModel(0, 10);
            var settings = ConfigurationHelper.GetExtensions(ExtensionsAppsettingKey.VideoExtensions);
            VideoExtensions = new ObservableCollection<string>(settings.Select(x => x.Extension));
            Model = new VideoSourceDestinationModel(settings.Where(x => x.DefaultSelected).Select(x => x.Extension));
        }

        public VideoCopyViewModel()
            : this(null, null,null,null, null, null, null, null)
        {

        }

        protected override void OnSelectionModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if (propertyName == nameof(SelectionModel.VideosWithSubtitlesOnly))
            {
                Model.IncludeSubtitles = SelectionModel.VideosWithSubtitlesOnly? true : Model.IncludeSubtitles;
            }
        }

        protected async override Task CopySpecificAsync(VideoFileRepresenter copyRepresenter, CancellationToken token)
        {
            if (Model.IncludeSubtitles)
            {                
                foreach (var subtitlePath in copyRepresenter.SubtitlePaths)
                {
                    var subtitleName = Path.GetFileName(subtitlePath);
                    using (var fileStream = File.Open(subtitlePath, FileMode.Open))
                    {
                        var destinationFilePath = Path.Combine(Model.DestinationPath, subtitleName);
                        using (var destinationStream = File.Create(destinationFilePath))
                        {
                            await fileStream.CopyToAsync(destinationStream, 81920, token);
                        }
                    }
                }
            }
            
        }

        protected override void MoveSpecific(VideoFileRepresenter copyRepresenter)
        {
            if (Model.IncludeSubtitles)
            {
                foreach (var subtitlePath in copyRepresenter.SubtitlePaths)
                {
                    var subtitleName = Path.GetFileName(subtitlePath);
                    File.Move(subtitlePath, Path.Combine(Model.DestinationPath, subtitleName));
                }
            }
        }



        protected override VideoFileRepresenter CreateFileRepresenter(FileInfo fileInfo)
        {
            return _fileRepresenterFactory.CreateVideoFileRepresenter(fileInfo, Model.IncludeSubtitles);
        }

        protected override Task SelectRandomFilesAsync(IEnumerable<VideoFileRepresenter> copyRepresenter, IEnumerable<MovedOrCopiedFile> copiedFileList,  CancellationToken token)
        {
            return _randomFileSelector.SelectMaximumAmountOfRandomFilesAsync(copyRepresenter,SelectionModel.MinimumFileSizeInBytes, SelectionModel.MaximumFileSizeInBytes, SelectionModel.SelectedSizeInBytes, SelectionModel.VideosWithSubtitlesOnly, copiedFileList, SelectionModel.AvoidDuplicates, token );
        }

        public ObservableCollection<string> VideoExtensions { get; set; }
    }
}
