using System.Collections.Generic;

namespace RandomFileCopier.Logic.Helper
{
    interface IListWithErrorDictionary<T>
        : IList<T>
    {
        Dictionary<string, string> Errors { get; }

        void AddError(string filePath, string error);
    }
}