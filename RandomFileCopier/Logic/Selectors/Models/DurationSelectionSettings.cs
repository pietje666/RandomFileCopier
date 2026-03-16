namespace RandomFileCopier.Logic
{
    class DurationSelectionSettings
    {
        public double? MinimumDuration { get; set; }
        public double? MaximumDuration { get; set; }
        public bool IncludeFilesWithoutDuration { get; set; } = true;
    }
}
