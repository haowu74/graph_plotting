using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class Reading
    {
        public int Id { get; set; }

        public int SignalStrength { get; set; }

        public int PulseWaveform { get; set; }

        public int Pulse { get; set; }

        public int Spo2 { get; set; }

        public DateTime TimeStamp { get; set; }

        public Reading(int id, int spo2, int pulse, int wave, int ss, DateTime dt)
        {
            Id = id;
            Spo2 = spo2;
            Pulse = pulse;
            PulseWaveform = wave;
            SignalStrength = ss;
            TimeStamp = dt;
        }
    }
}
