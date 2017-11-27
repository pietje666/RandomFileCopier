using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    interface IRandomFolderSelector
    {
        Task SelectMaximumAmountOfRandomFoldersAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize,IEnumerable<CopiedFile> copiedFileList, CancellationToken token);
    }
}
