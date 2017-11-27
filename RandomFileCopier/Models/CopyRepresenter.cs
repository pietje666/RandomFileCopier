using System;
using GalaSoft.MvvmLight;

namespace RandomFileCopier.Models
{
    class CopyRepresenter
        : ObservableObject
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CopyRepresenter(string path, string name, long size)
        {
            Path = path;
            Name = name;
            Size = size;
            Guid = Guid.NewGuid();
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; RaisePropertyChanged(); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private long _size;

        public long Size
        {
            get { return _size; }
            set
            {
                _size = value;
                RaisePropertyChanged();
                SizeInMB = (Size / 1024.0) / 1024.0;
                SizeInGB = ((Size / 1024.0) / 1024.0) / 1024.0;
            }
        }

        private double _sizeInMB;

        public double SizeInMB
        {
            get { return _sizeInMB; }
            set { _sizeInMB = value; RaisePropertyChanged(); }
        }

        private double _sizeInGb;

        public double SizeInGB
        {
            get { return _sizeInGb; }
            set { _sizeInGb = value; RaisePropertyChanged(); }
        }

        public Guid Guid { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(); }
        }

    }
}
