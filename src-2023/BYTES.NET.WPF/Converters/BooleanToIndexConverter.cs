//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BYTES.NET.WPF.Converters
{
    /// <summary>
    /// a converter class, suitablee for binding to a boolean value via XAML combobox
    /// </summary>
    /// <remarks>based on the article found at 'https://stackoverflow.com/questions/4335339/how-to-bind-a-boolean-to-combobox-in-wpf'</remarks>
    public class BooleanToIndexConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value == 0) ? true : false;
        }
    }
}
