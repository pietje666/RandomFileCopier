using System.Collections.Generic;
using RandomFileCopier.Models.Base;

namespace RandomFileCopier.Models
{
    class VideoSourceDestinationModel
        : SourceDestinationModel<VideoFileRepresenter>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VideoSourceDestinationModel(IEnumerable<string> extensions)
            : base(extensions)
        {
            IncludeSubtitles = true;
        }

        private bool _includeSubTitles;

        public bool IncludeSubtitles
        {
            get { return _includeSubTitles; }
            set { _includeSubTitles = value; RaisePropertyChanged(); }
        }
    }
}
