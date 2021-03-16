using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public class SaveFileDialogProperties : IDialogProperties
    {
        public SaveFileDialogProperties(DIALOG_RESULT dialogResult, string title = "", string filter = "",
            string filename = "")
        {
            Title = title;
            Filter = filter;
            FileName = filename;
            DialogResult = dialogResult;
        }

        public string Title { get; set; }
        public string Filter { get; set; }
        public string FileName { get; set; }
        public DIALOG_RESULT DialogResult { get; set; }
    }
}
