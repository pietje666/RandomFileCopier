using RandomFileCopier.Models.Selection.Base;

namespace RandomFileCopier.Models.Selection
{
    class VideoSelectionModel
        : SelectionModel
    {

        public VideoSelectionModel(double minimumFileSize, double maximumFileSize)
            : base(minimumFileSize, maximumFileSize, UnitSize.GB)
        {
            
        }

        private bool _videosWithSubtitlesOnly;

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
