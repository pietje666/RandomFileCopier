using RandomFileCopier.Models.Selection.Base;

namespace RandomFileCopier.Models.Selection
{
    class VideoSelectionModel
        : MediaSelectionModel
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public VideoSelectionModel(double minimumFileSize, double maximumFileSize)
            : base(minimumFileSize, maximumFileSize, UnitSize.GB, 0, 180)
        {
            
        }

        private bool _videosWithSubtitlesOnly;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool VideosWithSubtitlesOnly
        {
            get { return _videosWithSubtitlesOnly; }
            set
            {
                _videosWithSubtitlesOnly = value;
                RaisePropertyChanged();
            }
        }
    }
}
