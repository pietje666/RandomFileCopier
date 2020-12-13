namespace RandomFileCopier.Helpers
{
    static class UnitConverter
    {
        public static long GigaByteToByteConverter(double size)
        {
            return (long)(((size * 1024) * 1024)*1024);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static long MegaByteToByteConverter(double size)
        {
            return (long)((size * 1024) * 1024);
        }

    }
}
