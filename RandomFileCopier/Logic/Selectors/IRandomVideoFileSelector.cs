using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    interface IRandomVideoFileSelector
    {
        Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<VideoFileRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, bool videosWithSubtitlesOnly, IEnumerable<CopiedFile> copiedFileList, CancellationToken token);
    }
}
