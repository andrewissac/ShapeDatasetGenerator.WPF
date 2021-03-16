using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public interface IDialogViewModel
    {
        DIALOG_RESULT Dialog_Result { get; set; }
        string Message { get; set; }
        event EventHandler<DialogCloseRequestedEventArgs> CloseDialogRequested;
    }
}
