//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BYTES.NET.WPF.Converters
{
    public class NothingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.ToString()))
            {
                // No parameter provided, check for null or empty value
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                // Parameter provided, convert parameter to boolean for comparison
                try
                {
                    bool comparing = System.Convert.ToBoolean(parameter);

                    if (comparing)
                    {
                        // Compare value with true condition
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        // Compare value with false condition
                        if (value != null)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Unable to convert value", ex);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
