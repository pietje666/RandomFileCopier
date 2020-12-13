using System.Collections.Generic;
using RandomFileCopier.Models.Base;

namespace RandomFileCopier.Models
{
    class AudioSourceDestinationModel
        : SourceDestinationModel<CopyRepresenter>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public AudioSourceDestinationModel(IEnumerable<string> extensions)
            : base(extensions)
        {
        }
    }
}
