using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public interface IDialogService
    {
        Window Owner { get; set; }
        void Register<TViewModel, TView>() where TViewModel : IDialogViewModel where TView : IDialogWindow;

        DIALOG_RESULT ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;
        Task<DIALOG_RESULT> ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;

        void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;
        Task ShowAsync<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel;

        string OpenFileDialog(string initialDirectory = "", string title = "Open file", string filename = "",
            string filter = "All files (*.*)|*.*", bool multiselect = false);

        IDialogProperties SaveFileDialog(string initialDirectory = "", string title = "Save file", string filename = "",
            string filter = "All files (*.*)|*.*");

        string FolderBrowserDialog(string initialDirectory = "", string title = "Select directory");
    }
}
