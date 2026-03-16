using RandomFileCopier.Logic.Selectors.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic
{
    interface IRandomVideoFileSelector
    {
        Task SelectMaximumAmountOfRandomFilesAsync(RandomVideoSelectionSettings randomVideoSelectionSettings, CancellationToken token);
    }
}
