using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public interface IDialogProperties
    {
        string Title { get; set; }
        string Filter { get; set; }
        string FileName { get; set; }
        DIALOG_RESULT DialogResult { get; set; }
    }
}
