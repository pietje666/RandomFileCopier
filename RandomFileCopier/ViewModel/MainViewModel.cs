using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using RandomFileCopier.Models;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated from xaml")]
    class MainViewModel
        : ObservableRecipient
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainViewModel()
        {
            
            ViewModelsByMediaType = new Dictionary<MediaType, IHasDataContextSwitchingMethods>
            {
                {  MediaType.Video, new VideoCopyViewModel() },
                {  MediaType.Audio, new AudioCopyViewModel() },
                {  MediaType.Picture, new PictureCopyViewModel() },
                {  MediaType.Folder, new FolderCopyViewModel() }
            };
            MediaType = MediaType.Video;


            //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("es-ES");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }
        
        private void SetViewModel()
        {
            var vm = ViewModelsByMediaType[MediaType];

            ViewModel?.OnDataContextChanging();
            ViewModel = vm;
            ViewModel.OnDataContextChanged();
        }

        private MediaType _mediaType;

        public MediaType MediaType
        {
            get { return _mediaType; }
            set
            {
                _mediaType = value;
                SetViewModel();
                OnPropertyChanged();
            }
        }

        private IHasDataContextSwitchingMethods _viewModel;

        public IHasDataContextSwitchingMethods ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; OnPropertyChanged(); }
        }

        public Dictionary<MediaType, IHasDataContextSwitchingMethods> ViewModelsByMediaType { get; private set; }
    }
}
