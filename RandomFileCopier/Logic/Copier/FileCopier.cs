using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic.Copier
{
    class FileCopier 
        : IFileCopier
    {

        public async Task CopyFileAsync(string sourceFilePath, string fileName, string destinationPath, CancellationToken token)
        {
            if (!File.Exists(sourceFilePath))
            {
               throw new FileNotFoundException(("Source File does not exist or could not be found: " + sourceFilePath));
            }

            using (var fileStream = File.Open(sourceFilePath, FileMode.Open))
            {
                var destinationFilePath = Path.Combine(destinationPath, fileName);
                using (var destinationStream = File.Create(destinationFilePath))
                {
                    await fileStream.CopyToAsync(destinationStream, 81920, token);
                }
            }
        }
    }
}
