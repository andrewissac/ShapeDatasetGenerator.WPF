using System;
using System.Globalization;
using System.Windows.Data;

namespace ShapeDatasetGeneratorWPF.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NullReferenceException(typeof(BoolToOpacityConverter).Name +
                                                 " received null as input parameter");
            // check for right input type
            if (value.GetType() != typeof(bool))
            {
                var errormsg = typeof(BoolToOpacityConverter).Name + " received wrong input type: " + value.GetType() +
                               ", expected Type: " + typeof(bool);
                throw new ArgumentException(errormsg);
            }

            if ((bool) value)
                return 0.3;
            return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}