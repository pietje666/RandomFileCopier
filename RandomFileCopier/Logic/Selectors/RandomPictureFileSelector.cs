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
        public Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, IEnumerable<CopiedFile> copiedFileList, bool avoidDuplicates, CancellationToken cancellationToken)
        {
            return SelectMaximumAmountOfRandomFilesAsync(files, minimumFileSize, maximumFileSize, maximumSize, cancellationToken, copiedFileList, avoidDuplicates);
        }
    }
}
