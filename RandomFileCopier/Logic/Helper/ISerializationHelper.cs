using System.Collections.Generic;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Helper
{
    interface ISerializationHelper
    {
        IEnumerable<CopiedFile> GetCopiedFileList(string destinationPath);
        void WriteCopiedFileList(string destinationPath, IEnumerable<CopiedFile> copiedFileList);
    }
}