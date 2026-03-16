using System.Collections.Generic;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    class RandomAudioSelectionSettings
    {
        public List<CopyRepresenter> Files { get; set; } = new List<CopyRepresenter>();
        public FileSizeSelectionSettings FileSizeSelectionSettings { get; set; }
        public  DurationSelectionSettings DurationSelectionSettings { get; set; }
        public List<MovedOrCopiedFile> CopiedFileList { get; set; } = new List<MovedOrCopiedFile>();
        public bool AvoidDuplicates { get; set; }
    }
}
