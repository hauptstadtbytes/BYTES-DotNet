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
            if(parameter == null)
            {
                parameter = true;
            }

            if ((bool)parameter)
            {
                if(value == null)
                {
                    return Visibility.Visible;
                } else
                {
                    if(value.GetType() == typeof(string))
                    {
                        if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
                        {
                            return Visibility.Visible;
                        }
                    }

                    return Visibility.Collapsed;
                }

            } else
            {
               
                if (value == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    if (value.GetType() == typeof(string))
                    {
                        if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
                        {
                            return Visibility.Collapsed;
                        }
                    }

                    return Visibility.Visible;
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
