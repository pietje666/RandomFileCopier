using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    interface IRandomFolderSelector
    {
        Task SelectMaximumAmountOfRandomFoldersAsync(List<CopyRepresenter> files, FileSizeSelectionSettings fileSizeSelectionSettings, List<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token);
    }
}
