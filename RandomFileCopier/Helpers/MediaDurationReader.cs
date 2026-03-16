using System;
using System.IO;
using NLog;

namespace RandomFileCopier.Helpers
{
    static class MediaDurationReader
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static double? GetDurationInSeconds(string filePath)
        {
            double? result = null;
            try
            {
                Type shellType = Type.GetTypeFromProgID("Shell.Application");
                if (shellType != null)
                {
                    dynamic shellApp = Activator.CreateInstance(shellType);
                    string folderPath = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);

                    dynamic folder = shellApp.NameSpace(folderPath);
                    dynamic item = folder?.ParseName(fileName);
                    object durationValue = item?.ExtendedProperty("System.Media.Duration");
                    result = durationValue != null ? Convert.ToInt64(durationValue) / 10_000_000.0 : (double?)null;
                }
            }
            catch (Exception exc)
            {
                _logger.Log(LogLevel.Debug, exc, "Could not read media duration for file: {0}", filePath);
            }
            return result;
        }
    }
}
