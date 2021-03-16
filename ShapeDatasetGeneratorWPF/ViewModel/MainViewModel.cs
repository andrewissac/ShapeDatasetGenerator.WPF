using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ShapeDatasetGeneratorWPF.Enums;
using ShapeDatasetGeneratorWPF.Messages;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using WPFLocalizeExtension.Engine;

namespace ShapeDatasetGeneratorWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            Messenger.Default.Register<FlyoutSettingsMessage>(this, FlyoutSettingsMessageReceived);
        }

        private CultureInfo _cultureInfo = LocalizeDictionary.CurrentCulture;
        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { Set(ref _cultureInfo, value); }
        }

        private bool _isSettingsFlyoutOpen = false;
        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set { Set(ref _isSettingsFlyoutOpen, value); }
        }

        private void FlyoutSettingsMessageReceived(FlyoutSettingsMessage obj)
        {
            this.IsSettingsFlyoutOpen = !this.IsSettingsFlyoutOpen;
        }

        private RelayCommand _openSettingsFlyoutCommand;
        public RelayCommand OpenSettingsFlyoutCommand
        {
            get
            {
                return _openSettingsFlyoutCommand ?? (_openSettingsFlyoutCommand = new RelayCommand(
                    () =>
                    {
                        this.IsSettingsFlyoutOpen = !this.IsSettingsFlyoutOpen;
                    }
                ));
            }
        }

        private RelayCommand _selectedCultureChangedCommand;
        public RelayCommand SelectedCultureChangedCommand
        {
            get
            {
                return _selectedCultureChangedCommand ?? (_selectedCultureChangedCommand = new RelayCommand(
                    () =>
                    {
                        LocalizeDictionary.Instance.Culture = this.CultureInfo;
                    }
                ));
            }
        }

        private RelayCommand _openGithubPageCommand;
        public RelayCommand OpenGithubPageCommand
        {
            get
            {
                return _openGithubPageCommand ?? (_openGithubPageCommand = new RelayCommand(
                    () =>
                    {
                        System.Diagnostics.Process.Start("https://github.com/andrewissac?tab=repositories");
                    }
                ));
            }
        }

        private RelayCommand _clearCanvasCommand;
        public RelayCommand ClearCanvasCommand
        {
            get
            {
                return _clearCanvasCommand ?? (_clearCanvasCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new ClearCanvasMessage());
                    }
                ));
            }
        }

        private RelayCommand _saveShapesCommand;
        public RelayCommand SaveShapesCommand
        {
            get
            {
                return _saveShapesCommand ?? (_saveShapesCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new SaveOutputImagesMessage());
                    }
                ));
            }
        }

        private RelayCommand<KeyEventArgs> _previewKeyDownCommand;
        public RelayCommand<KeyEventArgs> PreviewKeyDownCommand
        {
            get
            {
                return _previewKeyDownCommand ?? (_previewKeyDownCommand = new RelayCommand<KeyEventArgs>(OnPreviewKeyDown));
            }
        }

        private void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Messenger.Default.Send(new ClearCanvasMessage());
            }
            else if(e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                Messenger.Default.Send(new SaveOutputImagesMessage());
            }
        }
    }
}