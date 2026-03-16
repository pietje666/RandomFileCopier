using RandomFileCopier.Models;
using System.Collections.Generic;

namespace RandomFileCopier.Logic.Selectors.Models
{
    class RandomVideoSelectionSettings
    {
        public List<VideoFileRepresenter> Files { get; set; } = new List<VideoFileRepresenter>();
        public List<MovedOrCopiedFile> CopiedFilesList { get; set; } = new List<MovedOrCopiedFile>();
        public FileSizeSelectionSettings FileSizeSelectionSettings { get; set; }
        public DurationSelectionSettings DurationSelectionSettings { get; set; }
        public bool VideosWithSubtitlesOnly { get; set; }
        public bool AvoidDuplicates { get; set; }        
    }
}
