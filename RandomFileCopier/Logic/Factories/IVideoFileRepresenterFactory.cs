using System.IO;
using RandomFileCopier.Models;

namespace RandomFileCopier.Logic
{
    interface IVideoFileRepresenterFactory
    {
        VideoFileRepresenter CreateVideoFileRepresenter(FileInfo fileInfo, bool includeSubtitles);
    }
}