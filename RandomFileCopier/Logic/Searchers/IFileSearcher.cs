using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RandomFileCopier.Logic
{
    interface IFileSearcher
    {
        IEnumerable<FileInfo> SearchFiles(string path, IEnumerable<string> extensions, SearchOption searchOption, CancellationToken cancellationToken);
    }
}