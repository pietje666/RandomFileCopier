namespace RandomFileCopier.Helpers
{
    static class UnitConverter
    {
        public static long GigaByteToByteConverter(double size)
        {
            return (long)(((size * 1024) * 1024)*1024);
        }

        public static long MegaByteToByteConverter(double size)
        {
            return (long)((size * 1024) * 1024);
        }

    }
}
