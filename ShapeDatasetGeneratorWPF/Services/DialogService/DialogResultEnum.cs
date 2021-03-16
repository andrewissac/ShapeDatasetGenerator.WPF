using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    // same results as in System.Windows.Forms.DialogResults, but using this won't need reference to System.Windows.Forms namespace for the WPF Client
    public enum DIALOG_RESULT
    {
        Abort = 3,
        Cancel = 2,
        Ignore = 5,
        No = 7,
        None = 0,
        OK = 1,
        Retry = 4,
        Yes = 6
    }
}
