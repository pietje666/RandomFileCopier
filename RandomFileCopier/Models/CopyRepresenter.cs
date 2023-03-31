using System;
using CommunityToolkit.Mvvm.ComponentModel;

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
            set { _path = value; OnPropertyChanged(); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private long _size;

        public long Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
                SizeInMB = (Size / 1024.0) / 1024.0;
                SizeInGB = ((Size / 1024.0) / 1024.0) / 1024.0;
            }
        }

        private double _sizeInMB;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public double SizeInMB
        {
            get { return _sizeInMB; }
            set { _sizeInMB = value; OnPropertyChanged(); }
        }

        private double _sizeInGb;

        public double SizeInGB
        {
            get { return _sizeInGb; }
            set { _sizeInGb = value; OnPropertyChanged(); }
        }

        public Guid Guid { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

    }
}
