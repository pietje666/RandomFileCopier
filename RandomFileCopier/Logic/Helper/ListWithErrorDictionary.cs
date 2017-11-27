using System.Collections.Generic;

namespace RandomFileCopier.Logic.Helper
{
    class ListWithErrorDictionary<T>
        : List<T>, IListWithErrorDictionary<T>
    {

        public ListWithErrorDictionary()
        {
            Errors = new Dictionary<string, string>();
        }

        public void AddError(string filePath, string error)
        {
            Errors.Add(filePath, error);
        }

        /// <summary>
        /// Errors containing the filepath and the actual error
        /// </summary>
        public Dictionary<string, string> Errors { get; private set; }
    }
}
