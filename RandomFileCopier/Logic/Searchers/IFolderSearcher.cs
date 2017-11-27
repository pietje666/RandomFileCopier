using System.Collections.Generic;
using System.IO;

namespace RandomFileCopier.Logic
{
    interface IFolderSearcher
    {
        IEnumerable<DirectoryInfo> SearchFolders(string path);

        long CalculateDirSize(DirectoryInfo directoryInfo);
    }
}
