using System;
using System.Windows;
using System.Windows.Input;

namespace HardwareMonitorPro
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries[0];
            var newTheme = currentTheme.Source.ToString().Contains("Dark")
                ? new Uri("/Themes/LightTheme.xaml", UriKind.Relative)
                : new Uri("/Themes/DarkTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries[0] =
                new ResourceDictionary { Source = newTheme };
        }

        private void RefreshData_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).Timer_Tick(null, null);
        }
        private OverlayWindow _overlay;

        private void ToggleOverlay_Click(object sender, RoutedEventArgs e)
        {
            if (_overlay == null)
            {
                _overlay = new OverlayWindow((MainViewModel)DataContext);
                _overlay.Closed += (s, args) => _overlay = null;
                _overlay.Show();
            }
            else
            {
                _overlay.Close();
                _overlay = null;
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}