using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Configuration
    {
        public static int TimeSpan { get; } = 6000; // 10 * 60 s = 10 min
        public static int WaveformPlotWidth { get; } = 6000;
        public static int WaveformPlotHeight { get; } = 80;
        public static int MainPlotWidth { get; } = 6000;
        public static int Spo2PlotHeight { get; } = 100;
        public static int PulsePlotHeight { get; } = 180;

    }
}
