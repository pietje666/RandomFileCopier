    using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using RandomFileCopier.Properties;

namespace RandomFileCopier.Validation
{
    internal class FolderValidator
         : IValidator<string>
    {
        public bool IsValid(string entity)
        {
            var valid = false;
            if (!string.IsNullOrEmpty(entity))
            {
                FileInfo fi = null;
                try
                {
                    fi = new FileInfo(entity);
                }
                catch (ArgumentException) { }
                catch (PathTooLongException) { }
                catch (NotSupportedException) { }
                valid = fi != null && Directory.Exists(entity);
            }
            return valid;
        }

        public string ErrorMessage { get { return  Resources.IncorrectDirectory; } }
    }
}
