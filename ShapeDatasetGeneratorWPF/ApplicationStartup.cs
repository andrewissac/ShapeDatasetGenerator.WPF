using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using WPFLocalizeExtension.Engine;
using ShapeDatasetGeneratorWPF.Views;
using ShapeDatasetGeneratorWPF.Models;
using ShapeDatasetGeneratorWPF.Settings;
using ShapeDatasetGeneratorWPF.Services.DialogService;

namespace ShapeDatasetGeneratorWPF
{
    public sealed class ApplicationStartup
    {

        /// <summary>
        ///  Main entry point method for application.
        /// </summary>
        [STAThread]
        public static void Main()
         {
            DispatcherHelper.Initialize();

            manualResetEventSplashScreen = new ManualResetEvent(false);
            applicationSplashScreenThread = new Thread(DisplayApplicationSplashScreen);

            applicationSplashScreenThread.SetApartmentState(ApartmentState.STA);
            applicationSplashScreenThread.IsBackground = true;
            applicationSplashScreenThread.Name = "SplashScreenThread";

            applicationSplashScreenThread.Start();
            manualResetEventSplashScreen.WaitOne();


            string appName = "ShapeDatasetGeneratorWPF" + "c5074625-f3cd-47a7-8db2-492082901ef5";
            bool createdNew;
            // ensure that only one instance of client is running at the same time (computer-wide!)
            // used a random Guid, just to make sure it is unique
            using (var mutex = new System.Threading.Mutex(false, appName, out createdNew))
            {
                var hasHandle = false;
                try
                {
                    try
                    {
                        hasHandle = mutex.WaitOne(TimeSpan.FromSeconds(1), false);
                        if (hasHandle == false)
                        {
                            var singleInstanceErrorWindow = new SingleInstanceErrorWindow();
                            singleInstanceErrorWindow.ShowDialog();
                            applicationSplashScreen.AppLoadingCompleted();
                            applicationSplashScreen = null;
                            //exit application
                            Environment.Exit(0);
                        }
                    }
                    catch (System.Threading.AbandonedMutexException)
                    {
                        // Log the fact that the mutex was abandoned in another process,
                        // it will still get acquired
                        hasHandle = true;
                    }

                    //Start Application logic

                    // The ViewModelLocator is usually instantiated via XAML in App.xaml
                    // It is needed here to get the IUserSettingsService, to fetch UserSettings before App.xaml is loaded
                    // therefor the usersettingsservice and usersettings are registered in the SimpleIoc before the viewmodellocator is instantiated
                    // Registration of global services and settings as singletons

                    #region Register Singletons

                    SimpleIoc.Default.Register<IDialogService, DialogService>();
                    var dialogService = SimpleIoc.Default.GetInstance<IDialogService>();


                    SimpleIoc.Default.Register<UserSettings>();
                    var userSettings = SimpleIoc.Default.GetInstance<UserSettings>();
                    // Map ViewModels to DialogWindows => expects <IDialogViewModel, IDialogWindow>
                    // BaseClass ValidatableDialogViewModel implements IDialogViewModel
                    // BaseClass DialogWindowBase implements IDialogWindow
                    // => every registered VM and Window has to inherit from these Base Classes
                    //dialogService.Register<YesNoMessageWindowViewModel, YesNoMessageWindow>();

                    RegisterBusinessLogicToSimpleIoc();

                    #endregion


                    // Set app localization
                    LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                    LocalizeDictionary.Instance.IncludeInvariantCulture = false;
                    LocalizeDictionary.Instance.Culture = new CultureInfo("en");

                    var application = new App(applicationSplashScreen, dialogService);
                    application.InitializeComponent();
                    application.Run();
                }
                finally
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }


        #region Variable Declaration

        /// <summary>
        ///     Thread that runs the application splash screen on application startup.
        /// </summary>
        private static Thread applicationSplashScreenThread;

        /// <summary>
        ///     Instance of application splash screen window.
        /// </summary>
        private static SplashScreenWindow applicationSplashScreen;

        /// <summary>
        ///     Manual reset event instance to hold the application splash screen thread till splash screen completes its loading.
        /// </summary>
        private static ManualResetEvent manualResetEventSplashScreen;

        //private static IUserSettings UserSettings { get; set; }
        //private static CGlobData GlobalData { get; set; }
        //private static IUserSettingsService userSettingsService;

        #endregion

        #region Static Methods

        /// <summary>
        ///     Display application splash screen.
        /// </summary>
        private static void DisplayApplicationSplashScreen()
        {
            applicationSplashScreen = new SplashScreenWindow();
            applicationSplashScreen.Show();
            manualResetEventSplashScreen.Set();
            Dispatcher.Run();
        }

        /// <summary>
        ///     Register business logic instances to SimpleIoc container
        ///     (Not in ViewModelLocator since business logic is View and ViewModel agnostic)
        /// </summary>
        private static void RegisterBusinessLogicToSimpleIoc()
        {

        }
        #endregion
    }

}
