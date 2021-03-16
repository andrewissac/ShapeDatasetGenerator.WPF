using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace ShapeDatasetGeneratorWPF.Services.DialogService
{
    public class DialogService : IDialogService
    {

        public DialogService()
        {
            Mappings = new Dictionary<Type, Type>();
            DialogsOpenCount = 0;
        }

        public int DialogsOpenCount { get; set; }

        public IDictionary<Type, Type> Mappings { get; }
        public Window Owner { get; set; }

        public void Register<TViewModel, TView>()
            where TViewModel : IDialogViewModel
            where TView : IDialogWindow
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public DIALOG_RESULT ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
        {
            var viewType = Mappings[typeof(TViewModel)];

            var res = DIALOG_RESULT.None;

            Application.Current.Dispatcher.Invoke(delegate
            {

                var dialog = (IDialogWindow)Activator.CreateInstance(viewType);
                EventHandler<DialogCloseRequestedEventArgs> handler = null;

                handler = (sender, e) =>
                {
                    viewModel.CloseDialogRequested -= handler;
                    // TODO: Refactor logic for more safety
                    dialog.Dialog_Result = e.Dialog_Result;

                    DialogsOpenCount -= 1;

                    dialog.Close();
                };

                viewModel.CloseDialogRequested += handler;

                dialog.DataContext = viewModel;
                dialog.Owner = Owner;

                DialogsOpenCount += 1;
                dialog.ShowDialog();
                res = dialog.Dialog_Result;

            });

            return res;
        }

        public async Task<DIALOG_RESULT> ShowDialogAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : IDialogViewModel
        {
            var res = DIALOG_RESULT.None;
            await DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => res = ShowDialog(viewModel)));
            return res;
        }


        public void Show<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
        {
            var viewType = Mappings[typeof(TViewModel)];

            var dialog = (IDialogWindow)Activator.CreateInstance(viewType);

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseDialogRequested -= handler;
                // TODO: Refactor logic
                dialog.Close();
            };

            viewModel.CloseDialogRequested += handler;

            dialog.DataContext = viewModel;
            // if owner is set => childwindows is always on top of owner.
            //dialog.Owner = this.Owner;
            dialog.Show();
        }

        public async Task ShowAsync<TViewModel>(TViewModel viewModel) where TViewModel : IDialogViewModel
        {
            await DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => Show(viewModel)));
        }

        public string OpenFileDialog(string initialDirectory = "", string title = "Open file", string filename = "",
            string filter = "All files (*.*)|*.*", bool multiselect = false)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;
            dialog.FileName = filename;
            dialog.Multiselect = multiselect;
            if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
                dialog.InitialDirectory = initialDirectory;
            else
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
                return dialog.FileName;
            return string.Empty;
        }

        public IDialogProperties SaveFileDialog(string initialDirectory = "", string title = "Save file",
            string filename = "", string filter = "All files (*.*)|*.*")
        {
            var dialog = new SaveFileDialog();
            dialog.Title = title;
            dialog.FileName = filename;
            dialog.Filter = filter;
            if (!string.IsNullOrWhiteSpace(initialDirectory) && Directory.Exists(initialDirectory))
                dialog.InitialDirectory = initialDirectory;
            else
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            // user pressed save
            if (dialog.ShowDialog() == true)
            {
                IDialogProperties returnProps =
                    new SaveFileDialogProperties(DIALOG_RESULT.OK, dialog.Title, dialog.Filter, dialog.FileName);
                return returnProps;
            }
            // user pressed cancel
            else
            {
                IDialogProperties returnProps = new SaveFileDialogProperties(DIALOG_RESULT.Cancel);
                return returnProps;
            }
        }

        public string FolderBrowserDialog(string initialDirectory = "", string title = "Select directory")
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = "";
            dialog.ShowNewFolderButton = true;
            dialog.Description = title;
            dialog.UseDescriptionForTitle = true;
            if (string.IsNullOrWhiteSpace(initialDirectory))
            {
                dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }
            else
            {
                dialog.SelectedPath = initialDirectory;
            }

            // user pressed save
            if (dialog.ShowDialog() == true)
            {
                return dialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}