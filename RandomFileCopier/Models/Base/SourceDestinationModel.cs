using System.Collections.Generic;
using System.Collections.ObjectModel;
using RandomFileCopier.Helpers;
using RandomFileCopier.Validation;

namespace RandomFileCopier.Models.Base
{
    internal class SourceDestinationModel<TCopyRepresenter>
        : ValidatableModelBase where TCopyRepresenter : CopyRepresenter
    {
        private IValidator<string> _folderValidator;

        private IValidator<string> _stringToDoubleValidator;

        private IEnumerable<string> _selectedExtensionsBackup;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification ="Raisepropertychanged not overriden")]
        public SourceDestinationModel(IEnumerable<string> extensions, IValidator<string> folderValidator, IValidator<string> stringToDoubleValidator)
        {
            _folderValidator = folderValidator ?? new FolderValidator();
            _stringToDoubleValidator = stringToDoubleValidator ?? new StringToDoubleValidator();
            
            IncludeSubDirectories = true;
            SelectedExtensions = new ObservableCollection<string>(extensions ?? new List<string>());
            
            Items = new ObservableCollection<TCopyRepresenter>();
        }

        public void BackExtensionsUp()
        {
            _selectedExtensionsBackup = new List<string>(SelectedExtensions);
        }

        public void RestoreSelectedExtensions()
        {
            if (_selectedExtensionsBackup != null)
            {
                //clear necesarry for a view without extensions, because then the itemssource doesnt get cleared => selected extensions neither
                SelectedExtensions.Clear();
                foreach (var item in _selectedExtensionsBackup)
                {
                    SelectedExtensions.Add(item);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SourceDestinationModel(IEnumerable<string> extensions)
            : this (extensions,  null,null)
        {
            
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SourceDestinationModel()
            : this( null,  null, null)
        {
        }

        private string _sourcePath;
        
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                Validate(value, _folderValidator);
                _sourcePath = value;
                RaisePropertyChanged();
            }
        }

        private string _destinationpath;

        public string DestinationPath
        {
            get { return _destinationpath; }
            set
            {
                Validate(value, _folderValidator);
                _destinationpath = value;
                RaisePropertyChanged();
            }
        }

        private double _maxDestinationSize;

        public double MaxDestinationSize
        {
            get { return _maxDestinationSize; }
            set { _maxDestinationSize = value; RaisePropertyChanged(); }
        }


        
        private bool _includeSubDirectories;

        public bool IncludeSubDirectories
        {
            get { return _includeSubDirectories; }
            set { _includeSubDirectories = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TCopyRepresenter> _items;

        public ObservableCollection<TCopyRepresenter> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }
        
        public ObservableCollection<string> SelectedExtensions { get; set; }
    }
}
