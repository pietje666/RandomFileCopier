using RandomFileCopier.Logic.Base;
using RandomFileCopier.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic
{
    class RandomAudioFileSelector
        : RandomFileSelectorBase<CopyRepresenter>, IRandomAudioFileSelector
    {

        public Task SelectMaximumAmountOfRandomFilesAsync(RandomAudioSelectionSettings randomAudioSelectionSettings, CancellationToken token)
        {
            var extraSelectors = new List<Func<CopyRepresenter, bool>>();

            if(randomAudioSelectionSettings.DurationSelectionSettings != null)
            {
                var durationSettings = randomAudioSelectionSettings.DurationSelectionSettings;
                extraSelectors.Add((file) =>
                    !file.DurationInSeconds.HasValue
                        ? durationSettings.IncludeFilesWithoutDuration
                        : file.DurationInSeconds.Value >= durationSettings.MinimumDuration && file.DurationInSeconds.Value <= durationSettings.MaximumDuration);
            }

            return SelectMaximumAmountOfRandomFilesAsync(randomAudioSelectionSettings.Files,
                                                        randomAudioSelectionSettings.FileSizeSelectionSettings,
                                                        token,
                                                        randomAudioSelectionSettings.CopiedFileList,
                                                        randomAudioSelectionSettings.AvoidDuplicates,
                                                        extraSelectors.ToArray());
        }
    }
}
