using System;
using LibreHardwareMonitor.Hardware;
using System.Linq;

namespace HardwareMonitorPro
{
    public class HardwareMonitorService
    {
        private readonly Computer _computer;

        public HardwareMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsStorageEnabled = true
            };
            _computer.Open();
        }

        public (float cpuLoad, float cpuTemp, float gpuLoad, float gpuTemp, float ramUsage) GetMetrics()
        {
            _computer.Accept(new UpdateVisitor());

            float cpuLoad = 0, cpuTemp = 0, gpuLoad = 0, gpuTemp = 0, ramUsage = 0;
            bool cpuTempFound = false;

            foreach (var hardware in _computer.Hardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    cpuLoad = hardware.Sensors
                        .FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("CPU Total"))?.Value ?? 0;

                    // Более надежный поиск температуры CPU
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && !cpuTempFound)
                        {
                            cpuTemp = sensor.Value ?? 0;
                            cpuTempFound = true;
                        }
                    }
                }

                if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                {
                    gpuLoad = hardware.Sensors
                        .FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("GPU Core"))?.Value ?? 0;
                    gpuTemp = hardware.Sensors
                        .FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name.Contains("GPU Core"))?.Value ?? 0;
                }

                if (hardware.HardwareType == HardwareType.Memory)
                {
                    ramUsage = hardware.Sensors
                        .FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("Memory"))?.Value ?? 0;
                }
            }

            return (cpuLoad, cpuTemp, gpuLoad, gpuTemp, ramUsage);
        }
    }

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer) => computer.Traverse(this);
        public void VisitHardware(IHardware hardware) => hardware.Update();
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
