/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ShapeDatasetGeneratorWPF"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ShapeDatasetGeneratorWPF.Services;

namespace ShapeDatasetGeneratorWPF.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsPageViewModel>();
            SimpleIoc.Default.Register<CanvasPageViewModel>();
            SimpleIoc.Default.Register<IFrameNavigationService>(SetupNavigation);
        }

        private static FrameNavigationService SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("SettingsPageViewModel", new Uri("../Views/SettingsPageViewModel.xaml", UriKind.Relative));
            return navigationService;
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public SettingsPageViewModel SettingsPageViewModel => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();

        public CanvasPageViewModel CanvasPageViewModel => ServiceLocator.Current.GetInstance<CanvasPageViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}