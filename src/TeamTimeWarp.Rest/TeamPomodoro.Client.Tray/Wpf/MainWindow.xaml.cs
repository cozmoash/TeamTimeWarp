using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace TimeManager.Client.Tray.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += HandleWindowLoaded;
        }

        private void HandleWindowLoaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
        }

    }
}
