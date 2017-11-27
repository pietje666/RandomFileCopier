using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic.Copier
{
    interface IFileCopier
    {
        Task CopyFileAsync(string sourceFilePath, string fileName, string destinationPath, CancellationToken token);
    }
}