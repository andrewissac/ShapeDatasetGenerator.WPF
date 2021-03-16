using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using ShapeDatasetGeneratorWPF.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShapeDatasetGeneratorWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private WindowState lastWindowState;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // this two line have to be exactly onload
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(new HwndSourceHook(WndProc));

            this.lastWindowState = this.WindowState;
        }

        const int WM_SIZING = 0x214;
        const int WM_EXITSIZEMOVE = 0x232;
        private static bool WindowWasResized = false;


        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SIZING)
            {
                if (WindowWasResized == false)
                {
                    WindowWasResized = true;
                }
            }

            if (msg == WM_EXITSIZEMOVE)
            { 
                if (WindowWasResized == true)
                {
                    // send message when resizing is finished
                    Messenger.Default.Send(new ResizedWindowMessage());
                    Debug.WriteLine("RESIZED WINDOW");
                    WindowWasResized = false;
                }
            }
            return IntPtr.Zero;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if ((this.lastWindowState == WindowState.Maximized && this.WindowState == WindowState.Normal) |
                (this.lastWindowState == WindowState.Normal && this.WindowState == WindowState.Maximized))
            {
                Messenger.Default.Send(new ResizedWindowMessage());
                Debug.WriteLine("MAXIMIZED OR RESTORED WINDOW");
                this.lastWindowState = this.WindowState;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
