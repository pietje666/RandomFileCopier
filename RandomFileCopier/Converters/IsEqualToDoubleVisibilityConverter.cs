using System;
using System.Globalization;
using System.Windows.Data;

namespace RandomFileCopier.Converters
{
    class IsEqualToDoubleVisibilityConverter
        : IValueConverter
    {
        public double Number { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = System.Windows.Visibility.Collapsed;
            if (value.GetType() == typeof(double))
            {
                visibility = (Number != (double)value) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
