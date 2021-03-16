using ShapeDatasetGeneratorWPF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShapeDatasetGeneratorWPF.UserControls
{
    /// <summary>
    ///     Interaction logic for BusySpinner.xaml
    /// </summary>
    public partial class BusySpinner : UserControl
    {
        public static readonly DependencyProperty SpinnerColorProperty = DependencyProperty.Register("SpinnerColor",
            typeof(SolidColorBrush), typeof(BusySpinner),
            new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 99, 99)),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty RotationSpeedProperty = DependencyProperty.Register("RotationSpeed",
            typeof(double), typeof(BusySpinner),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DotSizeProperty = DependencyProperty.Register("DotSize",
            typeof(double), typeof(BusySpinner),
            new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public BusySpinner()
        {
            InitializeComponent();
            //this.LoadViewFromUri("ShapeDatasetGeneratorWPF;component/usercontrols/busyspinner.xaml");
        }

        public SolidColorBrush SpinnerColor
        {
            get { return (SolidColorBrush)GetValue(SpinnerColorProperty); }
            set { SetValue(SpinnerColorProperty, value); }
        }

        public double RotationSpeed
        {
            get { return (double)GetValue(RotationSpeedProperty); }
            set { SetValue(RotationSpeedProperty, value); }
        }

        public double DotSize
        {
            get { return (double)GetValue(DotSizeProperty); }
            set { SetValue(DotSizeProperty, value); }
        }
    }
}
