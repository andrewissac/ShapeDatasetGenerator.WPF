using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShapeDatasetGeneratorWPF.Converters
{
    public class ExpanderRotateAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1.0;
            if (parameter is string parameterValue)
            {
                if (!double.TryParse(parameterValue.ToString(), out factor))
                {
                    factor = 1.0;
                }
            }
            switch (value)
            {
                case ExpandDirection.Left:
                     return 90 * factor;
                case ExpandDirection.Right:
                     return -90 * factor;
                default:
                     return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
