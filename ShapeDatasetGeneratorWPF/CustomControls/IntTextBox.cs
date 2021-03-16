using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShapeDatasetGeneratorWPF.CustomControls
{
    public class IntTextBox : TextBox
    {
        public IntTextBox()
        {
            this.PreviewTextInput += previewTextInput;
        }

        public static bool IsNumeric(string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(((IntTextBox)sender).Text + e.Text);
        }
    }
}
