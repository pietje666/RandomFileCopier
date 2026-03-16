using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Logic.Base;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Selectors
{
    class RandomPictureFileSelector
        : RandomFileSelectorBase<CopyRepresenter>, IRandomPictureFileSelector
    {
        public Task SelectMaximumAmountOfRandomFilesAsync(List<CopyRepresenter> files, FileSizeSelectionSettings fileSizeSelectionSettings, List<MovedOrCopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken cancellationToken)
        {
            return base.SelectMaximumAmountOfRandomFilesAsync(files, fileSizeSelectionSettings, cancellationToken, copiedFileList, avoidDuplicates);
        }
    }
}
