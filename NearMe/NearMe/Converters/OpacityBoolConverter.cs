using System;
using Xamarin.Forms;

namespace NearMe.Converters
{
    /*
     * Converitore
     * Vero = Non opaco
     * Falso = Opaco
     */
    class OpacityBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? 1 : 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value == 1 ? true : false;
        }
    }
}
