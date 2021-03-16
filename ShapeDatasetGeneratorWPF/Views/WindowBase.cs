using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ShapeDatasetGeneratorWPF.Views
{
    public class WindowBase : Window
    {
        private const uint WP_SYSTEMMENU = 0x02;
        private const uint WM_SYSTEMMENU = 0xa4;

        public WindowBase()
        {
            Loaded += OnLoaded;
        }

        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            var windowhandle = new WindowInteropHelper(this).Handle;
            var hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(WndProc);
        }

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == WM_SYSTEMMENU && wparam.ToInt32() == WP_SYSTEMMENU || msg == 165)
                //ShowContextMenu();
                handled = true;
            return IntPtr.Zero;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space)
                e.Handled = true;
            else
                base.OnKeyDown(e);
        }
    }
}
