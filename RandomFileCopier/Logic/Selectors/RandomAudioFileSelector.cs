using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Logic.Base;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    class RandomAudioFileSelector
        : RandomFileSelectorBase<CopyRepresenter>, IRandomAudioFileSelector
    {
        public Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<CopyRepresenter> files, long minimumFileSize, long maximumFileSize, long maximumSize, IEnumerable<CopiedFile> copiedFileList,  CancellationToken cancellationToken)
        {
           return base.SelectMaximumAmountOfRandomFilesAsync(files, minimumFileSize, maximumFileSize, maximumSize, cancellationToken, copiedFileList);
        }
    }
}
