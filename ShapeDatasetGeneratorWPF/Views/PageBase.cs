
using System.Windows;
using System.Windows.Controls;
using ShapeDatasetGeneratorWPF.ViewModel;

namespace ShapeDatasetGeneratorWPF.Views
{
    public class PageBase : Page
    {
        private INavigationAware NavigationAwareVM;

        public PageBase()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationAwareVM = (INavigationAware)DataContext;
            if (NavigationAwareVM != null) NavigationAwareVM.OnNavigatedTo();
        }


        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NavigationAwareVM = (INavigationAware)DataContext;
            if (NavigationAwareVM != null) NavigationAwareVM.OnNavigatedFrom();
        }
    }
}