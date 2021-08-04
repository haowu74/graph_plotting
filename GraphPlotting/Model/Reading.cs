using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Reading
    {
        public string DeviceId { get; set; }

        public int SignalStrength { get; set; }

        public int PulseWaveform { get; set; }

        public int Pulse { get; set; }

        public int Spo2 { get; set; }

        public Reading(string id, int spo2, int pulse, int wave, int ss)
        {
            DeviceId = id;
            Spo2 = spo2;
            Pulse = pulse;
            PulseWaveform = wave;
            SignalStrength = ss;
        }
    }
}
