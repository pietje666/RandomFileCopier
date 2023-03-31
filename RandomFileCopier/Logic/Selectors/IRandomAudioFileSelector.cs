using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    interface IRandomAudioFileSelector
    {
        Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, IEnumerable<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token);
    }
}
