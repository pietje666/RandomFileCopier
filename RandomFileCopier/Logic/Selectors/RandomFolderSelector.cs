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
        public Task SelectMaximumAmountOfRandomFoldersAsync(List<CopyRepresenter> files, FileSizeSelectionSettings fileSizeSelectionSettings, List<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken token)
        {
            return base.SelectMaximumAmountOfRandomFilesAsync(files, fileSizeSelectionSettings, token, copiedFileList, avoidDuplicates);
        }
    }
}
