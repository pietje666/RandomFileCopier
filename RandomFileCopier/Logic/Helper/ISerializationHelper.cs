using System.Collections.Generic;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Helper
{
    interface ISerializationHelper
    {
        IEnumerable<MovedOrCopiedFile> GetCopiedFileList(string destinationPath);
        void WriteCopiedFileList(string destinationPath, IEnumerable<MovedOrCopiedFile> copiedFileList);
    }
}