using System;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using ShapeDatasetGeneratorWPF.Services.DialogService;
using ShapeDatasetGeneratorWPF.Views;

namespace ShapeDatasetGeneratorWPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int secondsToShowSplashScreen = 5;

        /// <summary>
        ///     Application splash screen loading timer.
        /// </summary>
        private static Timer applicationSplashScreenTimer;
        private readonly IDialogService _dialogService;

        private SplashScreenWindow appSplashScreen;

        public App(SplashScreenWindow splashScreen, IDialogService dialogService)
        {
            appSplashScreen = splashScreen;
            _dialogService = dialogService;
            applicationSplashScreenTimer = new Timer(secondsToShowSplashScreen * 1000);
            applicationSplashScreenTimer.Elapsed += applicationSplashScreenTimer_Elapsed;
            applicationSplashScreenTimer.Start();
        }


        /// <summary>
        ///     Event handler for Application splash screen timer time elapsed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="eventArgs">Event arguments.</param>
        private void applicationSplashScreenTimer_Elapsed(object sender, ElapsedEventArgs eventArgs)
        {
            applicationSplashScreenTimer.Stop();
            applicationSplashScreenTimer.Dispose();
            applicationSplashScreenTimer = null;

            appSplashScreen.AppLoadingCompleted();
            appSplashScreen = null;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(StartMainApplication));
        }

        /// <summary>
        ///     Start application main window.
        /// </summary>
        private void StartMainApplication()
        {
            MainWindow = new MainWindow();
            _dialogService.Owner = MainWindow;

            // The data binding is done after the first windows is created. That makes issues on the property "Center Screen" for the main window
            // No real resolution on this yet but it can work around the problem using a dummy window
            var EmptyWin = new EmptyWindow();
            EmptyWin.Show();

            MainWindow.Show();

            EmptyWin.Hide();

            // force it on the foreground
            MainWindow.Activate();
            MainWindow.Topmost = true;
            MainWindow.Topmost = false;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void Application_Shutdown(object sender, ExitEventArgs e)
        {
            //this._userSettingsService.Save(this.GlobalData.UserSettings);
        }
    }
}
