using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Configuration
    {
        public static int WaveformPlotWidth { get; } = 600;
        public static int WaveformPlotHeight { get; } = 80;
        public static int MainPlotWidth { get; } = 600;
        public static int Spo2PlotHeight { get; } = 100;
        public static int PulsePlotHeight { get; } = 180;
        public static int DummyValue { get; } = -1000;
    }
}
