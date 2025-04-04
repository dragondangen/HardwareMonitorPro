using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace HardwareMonitorPro
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HardwareMonitorService _monitor;
        private readonly DispatcherTimer _timer;

        private float _cpuLoad;
        private float _cpuTemp;
        private float _gpuLoad;
        private float _gpuTemp;
        private float _ramUsage;

        public float CpuLoad
        {
            get => _cpuLoad;
            set { _cpuLoad = value; OnPropertyChanged(); }
        }

        public float CpuTemp
        {
            get => _cpuTemp;
            set { _cpuTemp = value; OnPropertyChanged(); }
        }

        public float GpuLoad
        {
            get => _gpuLoad;
            set { _gpuLoad = value; OnPropertyChanged(); }
        }

        public float GpuTemp
        {
            get => _gpuTemp;
            set { _gpuTemp = value; OnPropertyChanged(); }
        }

        public float RamUsage
        {
            get => _ramUsage;
            set { _ramUsage = value; OnPropertyChanged(); }
        }

        public string CpuText => $"CPU: {CpuLoad}% ({CpuTemp}°C)";
        public string GpuText => $"GPU: {GpuLoad}% ({GpuTemp}°C)";
        public string RamText => $"RAM: {RamUsage}%";

        public string CompactCpuText => $"CPU: {(int)CpuLoad}% ({(int)CpuTemp}°C)";
        public string CompactGpuText => $"GPU: {(int)GpuLoad}% ({(int)GpuTemp}°C)";
        public string CompactRamText => $"RAM: {(int)RamUsage}%";

        public MainViewModel()
        {
            _monitor = new HardwareMonitorService();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            var metrics = _monitor.GetMetrics();

            CpuLoad = metrics.cpuLoad;
            CpuTemp = metrics.cpuTemp;
            GpuLoad = metrics.gpuLoad;
            GpuTemp = metrics.gpuTemp;
            RamUsage = metrics.ramUsage;

            OnPropertyChanged(nameof(CpuText));
            OnPropertyChanged(nameof(GpuText));
            OnPropertyChanged(nameof(RamText));

            OnPropertyChanged(nameof(CompactCpuText));
            OnPropertyChanged(nameof(CompactGpuText));
            OnPropertyChanged(nameof(CompactRamText));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}