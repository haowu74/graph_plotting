using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Configuration
    {
        public static int TimeSpan { get; } = 600; // 10 min for 600 points
        public static int WaveformTimeSpan { get; } = 10; // 10 s for 600 points
        public static int WaveformPlotWidth { get; } = 600;
        public static int WaveformPlotHeight { get; } = 80;
        public static int MainPlotWidth { get; } = 600;
        public static int Spo2PlotHeight { get; } = 100;
        public static int PulsePlotHeight { get; } = 180;
    }
}
