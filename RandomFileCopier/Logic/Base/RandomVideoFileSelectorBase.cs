using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomFileCopier.Logic.Helper;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Base
{
    abstract class RandomFileSelectorBase<T> where T : CopyRepresenter
    {

        private static long SelectFilesThatFit(long maximumSize, List<T> orderedFiles, CancellationToken token,IEnumerable<Func<T, bool>> extraSelectors, long selectedSize = 0, Action<T> action = null)
        {
            int index = 0;
            //add files while size doesnt exceed total size
            while (selectedSize < maximumSize && index < orderedFiles.Count)
            {
                token.ThrowIfCancellationRequested();
                var file = orderedFiles[index];
                //file.Size >= minimumFileSize && file.Size <= maximumFileSize)
                if ( (extraSelectors == null) || (extraSelectors != null && extraSelectors.All(x => x(file))))
                { 
                    file.IsSelected = true;
                    selectedSize += orderedFiles[index].Size;
                    action?.Invoke(file);
                }
                index++;
            }

            //remove last file that has been added since then it would exceed total filesize
            if (index > 0 && selectedSize > maximumSize && orderedFiles[index - 1].IsSelected)
            {
                selectedSize -= orderedFiles[index - 1].Size;
                orderedFiles[index - 1].IsSelected = false;
            }
            return selectedSize;
        }

        protected static Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<T> files, long minimumFileSize, long maximumFileSize, long maximumSize, CancellationToken token, IEnumerable<CopiedFile> copiedFileList, params Func<T, bool>[] extraSelectors)
        {
            return Task.Run(() => { 
                token.ThrowIfCancellationRequested();
                IEnumerable<T> orderedFiles = files.OrderBy(x => x.Guid);
            
                var filesCopy = orderedFiles.ToList();
                var selectorsList = (extraSelectors ?? new Func<T, bool>[0]).ToList();
                selectorsList.Add((x) => MinMaxSelector(minimumFileSize, maximumFileSize, x));

                if (copiedFileList != null && copiedFileList.Any())
                {
                    selectorsList.Add(x => !copiedFileList.Any(y => y.Name == x.Name  && y.Size == x.Size));
                }

                //initial selection
                var selectedSize = SelectFilesThatFit(maximumSize, orderedFiles.ToList(), token, selectorsList, action: x => filesCopy.Remove(x));

                var orderedFilesBySize = filesCopy.OrderBy(x => x.Size).ToList();
                //reselect files this time ordered by size so the maximum amount of files are being selected
                SelectFilesThatFit(maximumSize, orderedFilesBySize, token, selectorsList, selectedSize );
            });
        }

        private static bool MinMaxSelector(long minimumFileSize, long maximumFileSize, T fileRepresenter)
        {
            return fileRepresenter.Size >= minimumFileSize && fileRepresenter.Size <= maximumFileSize;
        }
    }
}
