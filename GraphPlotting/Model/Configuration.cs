using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Configuration
    {
        public static int TimeSpan { get; } = 600; // 10 * 60 s = 10 min
        public static int WaveformTimeSpan { get; } = 100; // 10 s
        public static int WaveformPlotWidth { get; } = 600;
        public static int WaveformPlotHeight { get; } = 80;
        public static int MainPlotWidth { get; } = 600;
        public static int Spo2PlotHeight { get; } = 100;
        public static int PulsePlotHeight { get; } = 180;
        public static int Factor { get; } = 10000000;
    }
}
