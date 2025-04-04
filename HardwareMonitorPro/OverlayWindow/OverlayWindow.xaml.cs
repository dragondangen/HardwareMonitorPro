using System.Windows;

namespace HardwareMonitorPro
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Позиционируем в правом верхнем углу
            Left = SystemParameters.WorkArea.Right - Width - 10;
            Top = 10;

        }
    }
}
