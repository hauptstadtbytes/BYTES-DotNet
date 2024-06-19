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
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter == null)
                {
                    // No parameter provided, check for Count or Length >= 1
                    if (value is System.Collections.ICollection collection && collection.Count >= 1)
                    {
                        return Visibility.Visible;
                    }
                    else if (value != null && value.GetType().IsArray && ((Array)value).Length >= 1)
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
                    // Parameter provided, check for Count or Length >= parameter
                    int paramValue;
                    if (int.TryParse(parameter.ToString(), out paramValue))
                    {
                        if (value is System.Collections.ICollection collection && collection.Count >= paramValue)
                        {
                            return Visibility.Visible;
                        }
                        else if (value != null && value.GetType().IsArray && ((Array)value).Length >= paramValue)
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
                        return Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
