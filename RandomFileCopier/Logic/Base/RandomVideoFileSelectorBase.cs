using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accessibility;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Base
{

    internal class IsDuplicateFileComparer : IEqualityComparer<CopyRepresenter>
    {
        public bool Equals(CopyRepresenter x, CopyRepresenter y)
        {
            return x.Name == y.Name && x.Size == y.Size;
        }

        public int GetHashCode([DisallowNull] CopyRepresenter obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.Name.GetHashCode() ^ obj.Size.GetHashCode();
        }
    }

    abstract class RandomFileSelectorBase<T> where T : CopyRepresenter
    {
        private readonly IEqualityComparer<CopyRepresenter> _duplicateFileComparer;

        public RandomFileSelectorBase(IEqualityComparer<CopyRepresenter> duplicateFileComparer)
        {
            _duplicateFileComparer = duplicateFileComparer ?? new IsDuplicateFileComparer();
        }

        public RandomFileSelectorBase()
            :this(null)
        {

        }

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

        protected Task SelectMaximumAmountOfRandomFilesAsync(IEnumerable<T> files, long minimumFileSize, long maximumFileSize, long maximumSize, CancellationToken token, IEnumerable<CopiedFile> copiedFileList, bool avoidDuplicates, params Func<T, bool>[] extraSelectors)
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
                if (avoidDuplicates)
                {
                    filesCopy = filesCopy.Distinct<T>(_duplicateFileComparer).ToList();
                }
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
