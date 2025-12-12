using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomFileCopier.Logic
{
    class FolderSearcher
        : IFolderSearcher
    {
        private bool IsInRecycleBin(string folderPath)
        {
            return folderPath.IndexOf("$RECYCLE.BIN", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public IEnumerable<DirectoryInfo> SearchFolders(string path)
        {
            var directeryInfo = new DirectoryInfo(path);
            return directeryInfo.EnumerateDirectories().Where(x => !IsInRecycleBin(x.FullName));
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
