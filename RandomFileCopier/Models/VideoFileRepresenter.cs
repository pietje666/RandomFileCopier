using System.Collections.Generic;

namespace RandomFileCopier.Models
{
    class VideoFileRepresenter
        : CopyRepresenter
    {
        public VideoFileRepresenter(string path, string name, long size, IEnumerable<string> subtitlePaths) 
            : base(path, name, size)
        {
            SubtitlePaths = subtitlePaths ?? new List<string>();
        }
        
        private IEnumerable<string> _subtitlePaths;

        public IEnumerable<string> SubtitlePaths
        {
            get { return _subtitlePaths; }
            set { _subtitlePaths = value; RaisePropertyChanged(); }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string SubtitlePath
        {
            get { return string.Join(", ", SubtitlePaths); }
        }
    }
}
