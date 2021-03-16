using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ShapeDatasetGeneratorWPF.Messages;
using ShapeDatasetGeneratorWPF.Models;
using ShapeDatasetGeneratorWPF.Services.DialogService;
using ShapeDatasetGeneratorWPF.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ShapeDatasetGeneratorWPF.ViewModel
{
    public class SettingsPageViewModel : ValidatableViewModelBase, INavigationAware
    {
        public SettingsPageViewModel(IDialogService dialogService, UserSettings userSettings)
        {
            this._dialogService = dialogService;
            this.UserSettings = userSettings;
            BindingOperations.EnableCollectionSynchronization(this.UserSettings.OutputImgDimensions, _outputImgDimensionsLock);
        }

        #region Fields
        private object _outputImgDimensionsLock = new object();
        private readonly IDialogService _dialogService;
        #endregion

        #region Properties
        private UserSettings _userSettings;
        public UserSettings UserSettings
        {
            get { return _userSettings; }
            set { Set(ref _userSettings, value); }
        }

        private OutputImgDimension _selectedOutputImgDimension;
        public OutputImgDimension SelectedOutputImgDimension
        {
            get => _selectedOutputImgDimension;
            set => Set(ref _selectedOutputImgDimension, value);
        }

        private int _outputImgWidth = 64;
        public int OutputImgWidth
        {
            get { return _outputImgWidth; }
            set
            {
                Set(ref _outputImgWidth, value);
                RaisePropertyChanged(() => OutputImgDimension);
            }
        }

        private int _outputImgHeight = 64;
        public int OutputImgHeight
        {
            get { return _outputImgHeight; }
            set
            {
                Set(ref _outputImgHeight, value);
                RaisePropertyChanged(() => OutputImgDimension);
            }
        }

        public OutputImgDimension OutputImgDimension => new OutputImgDimension(OutputImgWidth, OutputImgHeight);
        #endregion

        #region Methods
        public void OnNavigatedTo()
        {

        }

        public void OnNavigatedFrom()
        {

        }
        #endregion

        #region Commands
        private RelayCommand _addOutputImgDimensionCommand;
        public RelayCommand AddOutputImgDimensionCommand
        {
            get
            {
                return _addOutputImgDimensionCommand ?? (_addOutputImgDimensionCommand = new RelayCommand(
                    () =>
                    {
                        lock (_outputImgDimensionsLock)
                        {
                            if (!this.UserSettings.OutputImgDimensions.Contains(OutputImgDimension) && OutputImgWidth > 0 && OutputImgHeight > 0)
                            {
                                this.UserSettings.OutputImgDimensions.Add(OutputImgDimension);
                            }
                        }
                    }
                ));
            }
        }

        private RelayCommand _removeOutputImgDimensionCommand;
        public RelayCommand RemoveOutputImgDimensionCommand
        {
            get
            {
                return _removeOutputImgDimensionCommand ?? (_removeOutputImgDimensionCommand = new RelayCommand(
                    () =>
                    {
                        lock (_outputImgDimensionsLock)
                        {
                            if (this.SelectedOutputImgDimension != null && this.UserSettings.OutputImgDimensions.Contains(this.SelectedOutputImgDimension))
                            {
                                this.UserSettings.OutputImgDimensions.Remove(this.SelectedOutputImgDimension);
                            }
                        }
                    }
                ));
            }
        }

        private RelayCommand _openFolderDialogCommand;
        public RelayCommand OpenFolderDialogCommand
        {
            get
            {
                return _openFolderDialogCommand ?? (_openFolderDialogCommand = new RelayCommand(
                    () =>
                    {
                        var path = this._dialogService.FolderBrowserDialog();
                        if(!string.IsNullOrWhiteSpace(path) && path != this.UserSettings.RootOutputPath)
                        {
                            this.UserSettings.RootOutputPath = path;
                        }
                    }
                ));
            }
        }
        #endregion

    }
}
