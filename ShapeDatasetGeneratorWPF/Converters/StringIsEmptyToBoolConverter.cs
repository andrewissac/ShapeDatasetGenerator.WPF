using System;
using System.Globalization;
using System.Windows.Data;

namespace ShapeDatasetGeneratorWPF.Converters
{
    public class StringIsEmptyToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NullReferenceException(typeof(StringIsEmptyToBoolConverter).Name +
                                                 " received null as input parameter");
            // check for right input type
            if (value.GetType() != typeof(string))
            {
                var errormsg = typeof(StringIsEmptyToBoolConverter).Name + " received wrong input type: " +
                               value.GetType() + ", expected Type: " + typeof(string);
                throw new ArgumentException(errormsg);
            }

            var sValue = (string) value;
            return string.IsNullOrWhiteSpace(sValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}