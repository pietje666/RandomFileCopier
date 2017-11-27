using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic.Copier
{
    interface IFolderCopier
    {
        Task DirectoryCopyAsync(string sourceDirName, string destDirName, CancellationToken token);
    }
}