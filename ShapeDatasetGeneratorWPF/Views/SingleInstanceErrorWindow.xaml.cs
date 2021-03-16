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
using System.Windows.Shapes;

namespace ShapeDatasetGeneratorWPF.Views
{
    /// <summary>
    /// Interaction logic for SingleInstanceErrorWindow.xaml
    /// </summary>
    public partial class SingleInstanceErrorWindow : Window
    {
        public SingleInstanceErrorWindow()
        {
            InitializeComponent();
            this.TextBlock_Title.Text = "Error";
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            if (currentCulture.Name == "de-DE")
            {
                this.TextBlock_ErrorMsg.Text = LocalizationProvider.GetLocalizedValueForTargetCulture<string>("SRCRunning", currentCulture);
            }
            else
            {
                this.TextBlock_ErrorMsg.Text = LocalizationProvider.GetLocalizedValueForTargetCulture<string>("SRCRunning", new System.Globalization.CultureInfo("en"));
            }

        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
