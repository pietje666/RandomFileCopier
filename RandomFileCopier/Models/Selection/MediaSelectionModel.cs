namespace RandomFileCopier.Models.Selection.Base
{
    class MediaSelectionModel
        : SelectionModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "RaisePropertyChanged not overriden")]
        public MediaSelectionModel(double minimumFileSize, double maximumFileSize, UnitSize unitSize,
            double minimumDuration = 0, double maximumDuration = 60)
            : base(minimumFileSize, maximumFileSize, unitSize)
        {
            MinimumDuration = minimumDuration;
            MaximumDuration = maximumDuration;
            IncludeFilesWithoutDuration = true;
        }

        private double _minimumDuration;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public double MinimumDuration
        {
            get { return _minimumDuration; }
            set { _minimumDuration = value; RaisePropertyChanged(); }
        }

        private double _maximumDuration;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public double MaximumDuration
        {
            get { return _maximumDuration; }
            set { _maximumDuration = value; RaisePropertyChanged(); }
        }

        public double MinimumDurationInSeconds { get { return MinimumDuration * 60.0; } }

        public double MaximumDurationInSeconds { get { return MaximumDuration * 60.0; } }

        private bool _includeFilesWithoutDuration;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool IncludeFilesWithoutDuration
        {
            get { return _includeFilesWithoutDuration; }
            set { _includeFilesWithoutDuration = value; RaisePropertyChanged(); }
        }
    }
}
