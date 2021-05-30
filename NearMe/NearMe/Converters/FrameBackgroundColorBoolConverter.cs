using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NearMe.Converters
{
    /*
     * Converitore
     * Vero = LightGray
     * Falso = Resources["colorAccent"]
     */
    class FrameBackgroundColorBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.LightGray : Application.Current.Resources["colorAccent"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Color)value == Color.LightGray ? true : false;
        }
    }
}
