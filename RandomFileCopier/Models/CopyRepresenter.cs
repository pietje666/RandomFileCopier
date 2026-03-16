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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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

        private double? _durationInSeconds;

        public double? DurationInSeconds
        {
            get { return _durationInSeconds; }
            set
            {
                _durationInSeconds = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DurationFormatted));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string DurationFormatted
        {
            get
            {
                if (!_durationInSeconds.HasValue || _durationInSeconds.Value <= 0) return string.Empty;
                var span = System.TimeSpan.FromSeconds(_durationInSeconds.Value);
                if (span.TotalHours >= 1)
                    return string.Format("{0}:{1:D2}:{2:D2}", (int)span.TotalHours, span.Minutes, span.Seconds);
                return string.Format("{0}:{1:D2}", (int)span.TotalMinutes, span.Seconds);
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(); }
        }

    }
}
