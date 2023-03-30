using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Logic.Base;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    class RandomFolderSelector
        : RandomFileSelectorBase<CopyRepresenter>, IRandomFolderSelector
    {
        public Task SelectMaximumAmountOfRandomFoldersAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, IEnumerable<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token)
        {
            return SelectMaximumAmountOfRandomFilesAsync(files,  minimumFileSize, maximumFileSize, maximumSize, token, copiedFileList, avoidDuplicates);
        }
    }
}
