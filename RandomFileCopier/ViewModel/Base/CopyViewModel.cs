using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RandomFileCopier.Dialogs;
using RandomFileCopier.Helpers;
using RandomFileCopier.Logic.Helper;
using RandomFileCopier.Models;
using RandomFileCopier.Models.Base;
using RandomFileCopier.Models.Selection.Base;
using RandomFileCopier.Properties;

namespace RandomFileCopier.ViewModel.Base
{

    internal abstract class CopyViewModel<TModel, TSelectionModel, TCopyRepresenter>
            : ViewModelBase, IDisposable, IHasDataContextSwitchingMethods
        where TModel : SourceDestinationModel<TCopyRepresenter>
        where TCopyRepresenter : CopyRepresenter
        where TSelectionModel : SelectionModel
    {
        private IDialogService _dialogService;
        private bool _isSliderLoaded;
        private bool _dragStarted;
        private List<string> _selectedExtensionsBackup;
        private SortDescription[] _backupSortDescriptors;
        private CancellationTokenSource _copyCancellationTokenSource;
        private CancellationTokenSource _searchCancellationTokenSource;
        private CancellationTokenSource _reselectFilesCancellationTokenSource;
        private readonly ISerializationHelper _serializationHelper;
        private readonly IOpenerHelper _openerHelper;


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        protected CopyViewModel(IDispatcherWrapper dispatcher, ISerializationHelper serializationHelper, IDialogService dialogService, IOpenerHelper openerHelper)
        {
            _isSliderLoaded = false;
            _dragStarted = false;
            _serializationHelper = serializationHelper;
            _dialogService = dialogService;
            _openerHelper = openerHelper;
            Dispatcher = dispatcher;
            SetCommands();
        }

        private void SetCommands()
        {

            BrowseSourceCommand = new RelayCommand(() =>
            {
                Model.RaisePropertyChanged(nameof(Model.SelectedExtensions));
                Model.SourcePath = _openerHelper.OpenDestinationFileDialog(true);
            });
            BrowseDestinationCommand = new RelayCommand(() => Model.DestinationPath = _openerHelper.OpenDestinationFileDialog());
            CopyCommand = new AsyncRelayCommand(CopyAsync, () => !IsBusySearching && Model.Items.Any(x => x.IsSelected) && TotalSelectedSize < SelectionModel.SelectedSize);
            MoveCommand = new AsyncRelayCommand(MoveAsync, () => !IsBusySearching && Model.Items.Any(x => x.IsSelected) && TotalSelectedSize < SelectionModel.SelectedSize);
            FindFilesCommand = new AsyncRelayCommand(FindFilesAsync, CanFindFiles);
            CancelFindFilesCommand = new RelayCommand(() => _searchCancellationTokenSource.Cancel(), () => IsBusySearching);
            CancelCopyCommand = new RelayCommand(() => _copyCancellationTokenSource.Cancel(), () => IsBusyCopying);
            SelectionClicked = new RelayCommand(HandleSelectionClicked);
            SliderDragCompletedCommand = new RelayCommand(SliderDragCompletedHandler);
            SliderDragStartedCommand = new RelayCommand(SliderDragStartedHandler);
            SliderValueChangedCommand = new RelayCommand(SliderValueChangedHandler);
            OpenFolderCommand = new RelayCommand(() => _openerHelper.OpenFolderHandler(SelectedItem), () => SelectedItem != null);
            RefreshSelectionCommand = new RelayCommand(() => ReSelectRandomFiles(Model.Items));
        }

        private async Task MoveAsync()
        {
            IsBusyMoving = true;
            FindFilesCommand.RaiseCanExecuteChanged();
            IListWithErrorDictionary<MovedOrCopiedFile> movedFiles = null;
            try
            {
                movedFiles = await MoveSpecificAsync();

            }          
            finally
            {
                Model.Items.Clear();
                IsBusyMoving = false;
                FindFilesCommand.RaiseCanExecuteChanged();
            }
            ShowCopyFinishedMessage(movedFiles != null && !movedFiles.Errors.Any(), Resources.SuccessMove , Resources.SomeFilesHaveNotBeenMoved );
            MoveCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand BrowseSourceCommand { get; private set; }
        public RelayCommand BrowseDestinationCommand { get; private set; }
        public AsyncRelayCommand FindFilesCommand { get; private set; }
        public RelayCommand CancelFindFilesCommand { get; private set; }
        public AsyncRelayCommand CopyCommand { get; private set; }
        public AsyncRelayCommand MoveCommand { get; private set; }
        public RelayCommand SelectionClicked { get; private set; }
        public RelayCommand SliderDragCompletedCommand { get; private set; }
        public RelayCommand SliderDragStartedCommand { get; private set; }
        public RelayCommand SliderValueChangedCommand { get; private set; }
        public RelayCommand CancelCopyCommand { get; private set; }
        public RelayCommand OpenFolderCommand { get; private set; }
        public RelayCommand RefreshSelectionCommand { get; private set; }
        public RelayCommand SliderLoadedCommand { get; private set; }
        public RelayCommand SliderUnloadedCommand { get; private set; }

        protected abstract Task SelectRandomFilesAsync(IEnumerable<TCopyRepresenter> copyRepresenterList, IEnumerable<MovedOrCopiedFile> copiedFileList, CancellationToken token);
        protected abstract Task SpecificSearchAsync(string path, CancellationToken token);
        protected abstract Task<IListWithErrorDictionary<MovedOrCopiedFile>> MoveSpecificAsync();
        protected abstract Task<IListWithErrorDictionary<MovedOrCopiedFile>> CopySpecificAsync(CancellationToken token);

        protected virtual void OnSelectionModelPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        protected IEnumerable<MovedOrCopiedFile> GetFileListOrNullIfNotApplicable()
        {
            IEnumerable<MovedOrCopiedFile> copiedFileList = null;

            if (SelectionModel.AvoidPreviousCopied)
            {
                copiedFileList = PreviouslyCopiedFileList;
            }
            return copiedFileList;
        }

        public void OnDataContextChanging()
        {
            BackupExtensions();
            _isSliderLoaded = false;
            var count = CollectionViewSourceItems.SortDescriptions.Count;
            _backupSortDescriptors = new SortDescription[count];

            CollectionViewSourceItems.SortDescriptions.CopyTo(_backupSortDescriptors, 0);
        }

        public void OnDataContextChanged()
        {
            RestoreExtensions();
            _isSliderLoaded = true;
            foreach (var descriptor in _backupSortDescriptors ?? Enumerable.Empty<SortDescription>())
            {
                CollectionViewSourceItems.SortDescriptions.Add(descriptor);
            }
        }

        private async void ReSelectRandomFiles(IEnumerable<TCopyRepresenter> copyRepresenterList)
        {
            try
            {
                _reselectFilesCancellationTokenSource?.Cancel();
                _reselectFilesCancellationTokenSource = new CancellationTokenSource();
                foreach (var item in Model.Items)
                {
                    item.IsSelected = false;
                    item.Guid = Guid.NewGuid();
                }

                var copiedFileList = GetFileListOrNullIfNotApplicable();

                await SelectRandomFilesAsync(copyRepresenterList, copiedFileList, _reselectFilesCancellationTokenSource.Token);
            }
            catch (TaskCanceledException) { }
            finally
            {
                HandleSelectionClicked();
                CollectionViewSourceItems.Refresh();
            }
        }

        private void CalculateNewMaxDestinationSize()
        {
            Task.Run(() =>
            {
                ulong freeBytes = 0;
                NativeMethods.DriveFreeBytes(Model.DestinationPath, out freeBytes);
                Model.MaxDestinationSize = (double)((freeBytes / 1024) / 1024) / 1024;
                SelectionModel.SelectedSize = Model.MaxDestinationSize; ;
            });
        }

        private void HandleSelectionClicked()
        {
            RaisePropertyChanged(() => SelectedFilesCount);
            CalculateTotalSelectedSize();
            CopyCommand.RaiseCanExecuteChanged();
            MoveCommand.RaiseCanExecuteChanged();
        }

        protected Task FindFilesAsync()
        {
            return Task.Run(async () =>
             {
                 if (!Model.HasErrors)
                 {
                     Dispatcher.Invoke(() => IsBusySearching = true);
                     Dispatcher.Invoke(CopyCommand.RaiseCanExecuteChanged);
                     Dispatcher.Invoke(MoveCommand.RaiseCanExecuteChanged);


                     _searchCancellationTokenSource = new CancellationTokenSource();
                     ClearPreviousSearchResults();

                     var path = Model.SourcePath;
                     try
                     {
                         await SpecificSearchAsync(path, _searchCancellationTokenSource.Token);
                     }
                     catch (OperationCanceledException) { }
                     finally
                     {
                         Dispatcher.Invoke(() =>
                         {
                             SetDefaultSortDescriptors();
                             CollectionViewSourceItems.Refresh();
                         });

                         RaisePropertyChanged(() => SelectedFilesCount);
                         CalculateTotalSelectedSize();
                         IsBusySearching = false;
                         Dispatcher.Invoke(CopyCommand.RaiseCanExecuteChanged);
                         Dispatcher.Invoke(MoveCommand.RaiseCanExecuteChanged);

                     }
                 }
             });
        }

        protected void CalculateTotalSelectedSize()
        {
            TotalSelectedSize = Model.Items.Where(x => x.IsSelected).Sum(x => x.SizeInGB);
            SelectedSizeColor = TotalSelectedSize < SelectionModel.SelectedSize ? Brushes.Black : Brushes.Red;
        }

        private void OnSelectionModelSet()
        {
            _selectionModel.PropertyChanged += SelectionModel_PropertyChanged;
        }

        private void ClearPreviousSearchResults()
        {
            Dispatcher.Invoke(() =>
            {
                Model.Items.Clear();
                CalculateTotalSelectedSize();
            });
        }

        protected void BackupExtensions()
        {
            _selectedExtensionsBackup = new List<string>(Model.SelectedExtensions);
        }

        private void RestoreExtensions()
        {
            if (_selectedExtensionsBackup != null)
            {
                //clear necesarry for a view without extensions, because then the itemssource doesnt get cleared => selected extensions neither
                Model.SelectedExtensions.Clear();
                foreach (var item in _selectedExtensionsBackup)
                {
                    Model.SelectedExtensions.Add(item);
                }
            }

            Model.RaisePropertyChanged(nameof(Model.SelectedExtensions));
        }

        private bool CanFindFiles()
        {
            return !IsBusySearching && !IsBusyMoving && !IsBusyCopying && !string.IsNullOrEmpty(Model.SourcePath)
                && !string.IsNullOrEmpty(Model.DestinationPath)
                && !Model.HasPropertyErrors(nameof(Model.SourcePath))
                && !Model.HasPropertyErrors(nameof(Model.DestinationPath));
        }

        private void SliderDragCompletedHandler()
        {
            ReSelectRandomFiles(Model.Items);
            _dragStarted = false;
        }

        private void SliderDragStartedHandler()
        {
            _dragStarted = true;
        }


        private void SliderValueChangedHandler()
        {
            if (!_dragStarted && _isSliderLoaded)
            {
                ReSelectRandomFiles(Model.Items);
            }
        }

        private async Task CopyAsync()
        {
            IsBusyCopying = true;
            FindFilesCommand.RaiseCanExecuteChanged();
            _copyCancellationTokenSource = new CancellationTokenSource();
            IListWithErrorDictionary<MovedOrCopiedFile> copiedFiles = null;
            var cancelled = false;
            try
            {
                copiedFiles = await CopySpecificAsync(_copyCancellationTokenSource.Token);

                var tempList = PreviouslyCopiedFileList.ToList();
                tempList.AddRange(copiedFiles);
                PreviouslyCopiedFileList = tempList;
                _serializationHelper.WriteCopiedFileList(Model.DestinationPath, tempList);
            }
            catch (TaskCanceledException)
            {
                cancelled = true;
            }
            finally
            {
                Model.Items.Clear();
                IsBusyCopying = false;
                FindFilesCommand.RaiseCanExecuteChanged();
                CopyCommand.RaiseCanExecuteChanged();
            }

            if (!cancelled)
            {
                ShowCopyFinishedMessage(copiedFiles != null && !copiedFiles.Errors.Any(), Resources.Success, Resources.SomeFilesHaveNotBeenCopied);
            }
        }

        private void ShowCopyFinishedMessage(bool showSucces, string successMessage, string warningMessage)
        {
            if (showSucces)
            {
                _dialogService.ShowDialog(successMessage, Resources.Info, MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else
            {
                _dialogService.ShowDialog(warningMessage, Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SelectionModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSelectionModelPropertyChanged(sender, e);
        }

        private void OnModelSet()
        {
            Model.PropertyChanged += Model_PropertyChanged;
            CollectionViewSourceItems = CollectionViewSource.GetDefaultView(Model.Items);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if ((propertyName == nameof(Model.SourcePath) && Model.SourcePath != null && !Model.HasPropertyErrors(nameof(Model.SourcePath))))
            {
                FindFilesCommand.RaiseCanExecuteChanged();
            }
            else if (propertyName == nameof(Model.DestinationPath) && Model.DestinationPath != null && !Model.HasPropertyErrors(nameof(Model.DestinationPath)))
            {
                FindFilesCommand.RaiseCanExecuteChanged();
                CalculateNewMaxDestinationSize();
                PreviouslyCopiedFileList = _serializationHelper.GetCopiedFileList(Model.DestinationPath);
            }
        }

        private void SetDefaultSortDescriptors()
        {
            if (!CollectionViewSourceItems.SortDescriptions.Any())
            {
                CollectionViewSourceItems.SortDescriptions.Add(new SortDescription("IsSelected", ListSortDirection.Descending));
                CollectionViewSourceItems.SortDescriptions.Add(new SortDescription(nameof(CopyRepresenter.Name), ListSortDirection.Ascending));
                CollectionViewSourceItems.Refresh();
            }
        }

        public CopyRepresenter SelectedItem { get; set; }
        public ICollectionView CollectionViewSourceItems { get; private set; }
        public IDispatcherWrapper Dispatcher { get; private set; }
        public IEnumerable<MovedOrCopiedFile> PreviouslyCopiedFileList { get; private set; }

        public int SelectedFilesCount
        {
            get { return Model.Items.Count(x => x.IsSelected); }
        }

        private double _totalSelectedSize;

        public double TotalSelectedSize
        {
            get { return _totalSelectedSize; }
            set { _totalSelectedSize = value; RaisePropertyChanged(); }
        }

        private Brush _selectedSizeColor;

        public Brush SelectedSizeColor
        {
            get { return _selectedSizeColor; }
            set { _selectedSizeColor = value; RaisePropertyChanged(); }
        }

        private bool _isBusySearching;

        public bool IsBusySearching
        {
            get { return _isBusySearching; }
            set { _isBusySearching = value; RaisePropertyChanged(); }
        }

        private bool _isBusyCopying;

        public bool IsBusyCopying
        {
            get { return _isBusyCopying; }
            set { _isBusyCopying = value; RaisePropertyChanged(); }
        }

        private bool _isBusyMoving;

        public bool IsBusyMoving
        {
            get { return _isBusyMoving; }
            set { _isBusyMoving = value; RaisePropertyChanged(); }
        }


        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { _progress = value; RaisePropertyChanged(); }
        }

        private int _maxProgressBarValue;

        public int MaxProgressBarValue
        {
            get { return _maxProgressBarValue; }
            set { _maxProgressBarValue = value; RaisePropertyChanged(); }
        }

        private TModel _model;

        public TModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnModelSet();
                RaisePropertyChanged();
#if DEBUG
                var count = System.Environment.ProcessorCount;
                if (count == 8)
                {
                    //Model.SourcePath = @"C:\temp\Spider-man (3 films)";
                    //Model.DestinationPath = @"c:\temp";
                }
                else
                {
                    Model.SourcePath = @"D:\Downloads\films";
                    Model.DestinationPath = @"c:\test";
                }
#endif
            }
        }

        private TSelectionModel _selectionModel;

        public TSelectionModel SelectionModel
        {
            get { return _selectionModel; }
            set
            {
                _selectionModel = value;
                OnSelectionModelSet();
                RaisePropertyChanged();
            }
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _searchCancellationTokenSource?.Dispose();
                _copyCancellationTokenSource?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
