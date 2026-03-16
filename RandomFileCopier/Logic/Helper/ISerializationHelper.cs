using System.Collections.Generic;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic.Helper
{
    interface ISerializationHelper
    {
        List<MovedOrCopiedFile> GetCopiedFileList(string destinationPath);
        void WriteCopiedFileList(string destinationPath, List<MovedOrCopiedFile> copiedFileList);
    }
}