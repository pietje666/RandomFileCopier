using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Logic.Base;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    class RandomVideoFileSelector 
        : RandomFileSelectorBase<VideoFileRepresenter>, IRandomVideoFileSelector
    {
        public Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<VideoFileRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, bool videosWithSubtitlesOnly, IEnumerable<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token )
        {
            return SelectMaximumAmountOfRandomFilesAsync(files, minimumFileSize, maximumFileSize,maximumSize, token, copiedFileList, avoidDuplicates,
                   (file) => 
                   {
                        var shouldSelect = true;
                        if (videosWithSubtitlesOnly)
                        {
                            shouldSelect = file.SubtitlePaths.Any();
                        }
                        return shouldSelect;
                    }
                   );
        }
    }
}
