using RandomFileCopier.Models.Selection.Base;

namespace RandomFileCopier.Models.Selection
{
    class AudioSelectionModel
        : MediaSelectionModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public AudioSelectionModel(double minimumFileSize, double maximumFileSize)
            : base(minimumFileSize, maximumFileSize, UnitSize.MB, 0, 60)
        {
        }
    }
}
