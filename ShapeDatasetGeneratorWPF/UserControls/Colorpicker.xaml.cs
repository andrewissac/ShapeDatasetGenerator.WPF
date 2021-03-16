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
using Xceed.Wpf.Toolkit;

namespace ShapeDatasetGeneratorWPF.UserControls
{
    /// <summary>
    /// Interaction logic for Colorpicker.xaml
    /// </summary>
    public partial class Colorpicker : UserControl
    {
        private Dictionary<string, Color> pr0Colors = new Dictionary<string, Color>()
        {
            { "Richtiges Grau" , (Color)ColorConverter.ConvertFromString("#FF161618") },
            { "Bewaehrtes Orange" , (Color)ColorConverter.ConvertFromString("#FFEE4D2E") },
            { "Altes Pink" , (Color)ColorConverter.ConvertFromString("#FFFF0082") },
            { "Angenehmes Gruen" , (Color)ColorConverter.ConvertFromString("#FF1db992") },
            { "Olivgruen des Friedens" , (Color)ColorConverter.ConvertFromString("#FFBFBC06") },
            { "Mega episches Blau" , (Color)ColorConverter.ConvertFromString("#FF008FFF") },
            { "Gamb" , (Color)ColorConverter.ConvertFromString("#FFFC8833") },
            { "Schrift" , (Color)ColorConverter.ConvertFromString("#FFF2F5f4") },
            { "Grau" , (Color)ColorConverter.ConvertFromString("#FF888888") },
            { "Link" , (Color)ColorConverter.ConvertFromString("#FF75C0C7") },
            { "Gebannt" , (Color)ColorConverter.ConvertFromString("#FF444444") }
        };

        public Colorpicker()
        {
            InitializeComponent();

            this.colorPicker.StandardColors.Clear();

            foreach (var color in pr0Colors)
            {
                this.colorPicker.StandardColors.Add(new ColorItem(color.Value, color.Key));
            }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor",
            typeof(Color), typeof(Colorpicker), new FrameworkPropertyMetadata(Colors.White));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
    }
}
