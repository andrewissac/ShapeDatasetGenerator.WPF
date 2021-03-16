using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShapeDatasetGeneratorWPF.Converters
{
    public class InvertBoolToCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NullReferenceException(typeof(InvertBoolToCollapsedVisibilityConverter).Name +
                                                 " received null as input parameter");
            // check for right input type
            if (value.GetType() != typeof(bool))
            {
                var errormsg = typeof(InvertBoolToCollapsedVisibilityConverter).Name + " received wrong input type: " +
                               value.GetType() + ", expected Type: " + typeof(bool);
                throw new ArgumentException(errormsg);
            }

            var bValue = (bool) value;
            if (bValue)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}