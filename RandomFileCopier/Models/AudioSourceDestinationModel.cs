using System.Collections.Generic;
using RandomFileCopier.Models.Base;

namespace RandomFileCopier.Models
{
    class AudioSourceDestinationModel
        : SourceDestinationModel<CopyRepresenter>
    {
        public AudioSourceDestinationModel(IEnumerable<string> extensions)
            : base(extensions)
        {
        }
    }
}
