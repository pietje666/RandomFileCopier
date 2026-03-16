using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic
{
    interface IRandomAudioFileSelector
    {
        Task SelectMaximumAmountOfRandomFilesAsync(RandomAudioSelectionSettings randomAudioSelectionSettings, CancellationToken token);
    }
}
