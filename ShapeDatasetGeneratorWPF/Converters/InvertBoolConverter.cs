using System;
using System.Globalization;
using System.Windows.Data;

namespace ShapeDatasetGeneratorWPF.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NullReferenceException(typeof(InvertBoolConverter).Name +
                                                 " received null as input parameter");
            //if (targetType != typeof(bool))
            //    throw new InvalidOperationException("The target must be a boolean, received type: " + value.GetType());

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NullReferenceException(typeof(InvertBoolConverter).Name +
                                                 " received null as input parameter");
            //if (targetType != typeof(bool))
            //    throw new InvalidOperationException("The target must be a boolean, received type: " + value.GetType());

            return !(bool) value;
        }
    }
}