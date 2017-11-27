using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RandomFileCopier.Dialogs;
using RandomFileCopier.Models;
using RandomFileCopier.ViewModel.Base;

namespace RandomFileCopier.ViewModel
{
    class MainViewModel
        : ViewModelBase
    {
        private IDialogService _dialogService;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification ="RaisePropertyChangedNotOverridden")]
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            
            ViewModelsByMediaType = new Dictionary<MediaType, IHasDataContextSwitchingMethods>
            {
                {  MediaType.Video, new VideoCopyViewModel() },
                {  MediaType.Audio, new AudioCopyViewModel() },
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
                RaisePropertyChanged();
            }
        }

        private IHasDataContextSwitchingMethods _viewModel;

        public IHasDataContextSwitchingMethods ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; RaisePropertyChanged(); }
        }

        public Dictionary<MediaType, IHasDataContextSwitchingMethods> ViewModelsByMediaType { get; private set; }
    }
}
