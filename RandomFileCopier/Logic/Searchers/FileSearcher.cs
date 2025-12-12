using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;

namespace RandomFileCopier.Logic
{
    /// <summary>
    /// test git commit working
    /// </summary>
    class FileSearcher : IFileSearcher
    {
        private bool IsInRecycleBin(string filePath)
        {
            return filePath.IndexOf("$RECYCLE.BIN", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public IEnumerable<FileInfo> SearchFiles(string path, IEnumerable<string> extensions, SearchOption searchOption, CancellationToken cancellationToken)
        {
            try
            {
                var dirFiles = Enumerable.Empty<FileInfo>();
                var directeryInfo = new DirectoryInfo(path);
                cancellationToken.ThrowIfCancellationRequested();
                if (searchOption == SearchOption.AllDirectories)
                {
                    dirFiles = directeryInfo.EnumerateDirectories("*")
                                        .Where(d => !IsInRecycleBin(d.FullName))
                                        .SelectMany(x => SearchFiles(x.FullName, extensions, searchOption, cancellationToken));
                }
                var directoryInfo2 = new DirectoryInfo(path);
                return dirFiles.Concat(directoryInfo2.EnumerateFiles("*")
                    .Where(x => extensions.Contains(x.Extension) && !IsInRecycleBin(x.FullName)));
            }
            catch (Exception exc) when (exc is UnauthorizedAccessException || exc is SecurityException)
            {
                Console.WriteLine(path);
                return Enumerable.Empty<FileInfo>();
            }
        }
    }
}
