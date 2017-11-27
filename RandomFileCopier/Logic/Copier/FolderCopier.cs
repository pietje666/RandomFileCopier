using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic.Copier
{
    class FolderCopier : IFolderCopier
    {
        private readonly IFileCopier _fileCopier;

        public FolderCopier(IFileCopier fileCopier)
        {
            _fileCopier = fileCopier ?? new FileCopier();
        }

        public FolderCopier()
            : this(null)
        {

        }

        public async Task DirectoryCopyAsync(string sourceDirName, string destDirName, CancellationToken token)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);
            var combinedDestinationDir = Path.Combine(destDirName, dir.Name);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException( "Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(combinedDestinationDir))
            {
                Directory.CreateDirectory(combinedDestinationDir);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                await _fileCopier.CopyFileAsync(file.FullName, file.Name, combinedDestinationDir, token);
            }

            var dirs = dir.GetDirectories();
            foreach (var subdir in dirs)
            {
                token.ThrowIfCancellationRequested();
                await DirectoryCopyAsync(subdir.FullName, Path.Combine(destDirName, dir.Name ), token);
            }
        }
    }
}
