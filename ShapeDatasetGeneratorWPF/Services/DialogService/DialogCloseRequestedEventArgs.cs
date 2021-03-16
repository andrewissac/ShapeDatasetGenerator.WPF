using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(DIALOG_RESULT dialogResult)
        {
            Dialog_Result = dialogResult;
        }

        public DIALOG_RESULT Dialog_Result { get; }
    }
}
