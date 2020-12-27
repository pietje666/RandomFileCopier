using System.Collections.Generic;
using RandomFileCopier.Models.Base;

namespace RandomFileCopier.Models
{
    class PictureSourceDestinationModel
        : SourceDestinationModel<CopyRepresenter>
    {
        public PictureSourceDestinationModel(IEnumerable<string> extensions)
            : base(extensions)
        {
        }
    }
}
