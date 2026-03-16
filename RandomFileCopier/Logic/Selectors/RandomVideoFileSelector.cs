using RandomFileCopier.Logic.Base;
using RandomFileCopier.Logic.Selectors.Models;
using RandomFileCopier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RandomFileCopier.Logic
{
    class RandomVideoFileSelector 
        : RandomFileSelectorBase<VideoFileRepresenter>, IRandomVideoFileSelector
    {
        public Task SelectMaximumAmountOfRandomFilesAsync(RandomVideoSelectionSettings randomVideoSelectionSettings, CancellationToken token)
        {
            var extraSelectors = new List<Func<VideoFileRepresenter, bool>>();

            if(randomVideoSelectionSettings.VideosWithSubtitlesOnly)
            {
                extraSelectors.Add((file) => file.SubtitlePaths.Any());
            }

            if (randomVideoSelectionSettings.DurationSelectionSettings != null)
            {
                var durationSettings = randomVideoSelectionSettings.DurationSelectionSettings;
                extraSelectors.Add((file) =>
                    !file.DurationInSeconds.HasValue
                        ? durationSettings.IncludeFilesWithoutDuration
                        : file.DurationInSeconds.Value >= durationSettings.MinimumDuration && file.DurationInSeconds.Value <= durationSettings.MaximumDuration);
            }

            return SelectMaximumAmountOfRandomFilesAsync(randomVideoSelectionSettings.Files,
                                                         randomVideoSelectionSettings.FileSizeSelectionSettings,
                                                         token,
                                                         randomVideoSelectionSettings.CopiedFilesList,
                                                         randomVideoSelectionSettings.AvoidDuplicates,
                                                         extraSelectors.ToArray());

        }
    }
}
