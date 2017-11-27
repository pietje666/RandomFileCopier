using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RandomFileCopier.Models;

namespace RandomFileCopier.Converters
{
    class MediaTypeVisibilityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mediaType = (MediaType)value;
            return mediaType == MediaType.Video ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
