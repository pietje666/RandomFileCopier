using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    class VideoFileRepresenterFactory
        : IVideoFileRepresenterFactory
    {
        public VideoFileRepresenter CreateVideoFileRepresenter(FileInfo fileInfo, bool includeSubtitles)
        {
            IEnumerable<string> subTitlePaths = null;
            if (includeSubtitles)
            {
                var subTitleSearchString = string.Format(CultureInfo.InvariantCulture, "{0}*.srt", Path.GetFileNameWithoutExtension(fileInfo.Name));
                subTitlePaths = fileInfo.Directory.EnumerateFiles(subTitleSearchString).Select(y => y.FullName);
            }
            return new VideoFileRepresenter(fileInfo.FullName, fileInfo.Name, fileInfo.Length , subTitlePaths);
        }
    }
}
