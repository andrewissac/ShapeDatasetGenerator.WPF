using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public interface IDialogWindow
    {
        DIALOG_RESULT Dialog_Result { get; set; }
        object DataContext { get; set; }
        Window Owner { get; set; }
        void Close();
        bool? ShowDialog();
        void Show();
    }
}
