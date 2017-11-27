using GalaSoft.MvvmLight;
using RandomFileCopier.Helpers;

namespace RandomFileCopier.Models.Selection.Base
{
    enum UnitSize
    {
        MB = 0,
        GB =1
    }

    class SelectionModel
        : ObservableObject
    {
        private readonly UnitSize _unitSize;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification ="RaisepropertyChanged not overriden")]
        public SelectionModel(double minimumFileSize, double maximumFileSize, UnitSize unitSize)
        {
            AvoidPreviousCopied = true;
            _unitSize = unitSize;
            MinimumFileSize = minimumFileSize;
            MaximumFileSize = maximumFileSize; 
        }

        private double _selectedSize;

        public double SelectedSize
        {
            get { return _selectedSize; }
            set
            {
                _selectedSize = value; RaisePropertyChanged();
            }
        }

        public long SelectedSizeInBytes { get { return UnitConverter.GigaByteToByteConverter(SelectedSize); } }

        private double _minimumFileSize;

        public double MinimumFileSize
        {
            get { return _minimumFileSize; }
            set
            {
                _minimumFileSize = value;
                MinimumFileSizeInBytes = CalculateSizeInBytes(MinimumFileSize);
                RaisePropertyChanged();
            }
        }

        private double _maximumFileSize;

        public double MaximumFileSize
        {
            get { return _maximumFileSize; }
            set
            {
                _maximumFileSize = value; 
                _maximumFileSizeInBytes = CalculateSizeInBytes(MaximumFileSize);
                RaisePropertyChanged();
            }
        }

        private long _minimumFileSizeInBytes;

        public long MinimumFileSizeInBytes
        {
            get { return _minimumFileSizeInBytes; }
            set { _minimumFileSizeInBytes = value; RaisePropertyChanged(); }
        }

        private long _maximumFileSizeInBytes;

        public long MaximumFileSizeInBytes
        {
            get { return _maximumFileSizeInBytes; }
            set { _maximumFileSizeInBytes = value; RaisePropertyChanged(); }
        }

        private bool _avoidPreviousCopied;

        public bool AvoidPreviousCopied
        {
            get { return _avoidPreviousCopied; }
            set { _avoidPreviousCopied = value; RaisePropertyChanged(); }
        }

        private long CalculateSizeInBytes(double size)
        {
            long sizeInBytes = 0;
            switch (_unitSize)
            {
                case UnitSize.MB:
                    sizeInBytes = UnitConverter.MegaByteToByteConverter(size);
                    break;
                case UnitSize.GB:
                    sizeInBytes = UnitConverter.GigaByteToByteConverter(size);
                    break;
            }
            return sizeInBytes;
        }
    }
}
