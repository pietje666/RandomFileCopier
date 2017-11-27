using System.Collections.Generic;
using System.IO;

namespace RandomFileCopier.Logic
{
    class FolderSearcher
        : IFolderSearcher
    {
        public IEnumerable<DirectoryInfo> SearchFolders(string path)
        {
            var directeryInfo = new DirectoryInfo(path);
            return directeryInfo.EnumerateDirectories();
        }

        public long CalculateDirSize(DirectoryInfo directoryInfo)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = directoryInfo.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = directoryInfo.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += CalculateDirSize(di);
            }
            return size;
        }
    }
}
