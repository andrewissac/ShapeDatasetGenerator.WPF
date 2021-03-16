using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShapeDatasetGeneratorWPF.CustomControls
{
    public class ComboBoxKeyboardExpandable : ComboBox
    {
        public ComboBoxKeyboardExpandable()
        {
            PreviewKeyUp += HandleEnterKey;
            DropDownClosed += MoveFocusOnDropDownClosed;
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (!IsDropDownOpen)
                    IsDropDownOpen = true;
        }

        private void MoveFocusOnDropDownClosed(object sender, EventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}