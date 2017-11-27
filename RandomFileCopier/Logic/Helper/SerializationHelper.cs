using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using RandomFileCopier.Dialogs;
using RandomFileCopier.Models;
using RandomFileCopier.Properties;

namespace RandomFileCopier.Logic.Helper
{
    class SerializationHelper : ISerializationHelper
    {
        private const string JSONFILENAME = "RandomFileCopier.json";
        private readonly IDialogService _dialogService;
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public SerializationHelper(IDialogService service)
        {
            _dialogService = service ?? new DialogService() ;
        }

        public SerializationHelper()
            :this(null)
        {

        }

        public IEnumerable<CopiedFile> GetCopiedFileList(string destinationPath)
        {

            var copiedFileList = Enumerable.Empty<CopiedFile>();
            var jsonFileDestinationPath = Path.Combine(destinationPath, JSONFILENAME);
            if (File.Exists(jsonFileDestinationPath))
            {
                copiedFileList = JsonConvert.DeserializeObject<IEnumerable<CopiedFile>>(File.ReadAllText(jsonFileDestinationPath));
            }
            return copiedFileList;
        }

        public void WriteCopiedFileList(string destinationPath,IEnumerable<CopiedFile> copiedFileList)
        {
            try
            {
                var newDestinationPath = Path.Combine(destinationPath, JSONFILENAME);
                if (File.Exists(newDestinationPath))
                {
                    File.Delete(newDestinationPath);
                }
                
                File.WriteAllText(newDestinationPath, JsonConvert.SerializeObject(copiedFileList));
                File.SetAttributes(newDestinationPath, FileAttributes.Hidden);
            }
            catch (Exception exc) when (exc is IOException || exc is UnauthorizedAccessException)
            {
                _dialogService.ShowDialog(Resources.NoCopiedFileListFileWritten, Resources.Warning, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                _logger.Log(LogLevel.Error, exc, "Json writefile error");
            }
        }
    }
}
