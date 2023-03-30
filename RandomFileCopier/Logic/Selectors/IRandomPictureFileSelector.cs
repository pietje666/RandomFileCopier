using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Selectors
{
    interface IRandomPictureFileSelector
    {
        Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, IEnumerable<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token);
    }
}
