using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class DeviceReading
    {
        public int DeviceId { get; set; }

        public string DeviceIdLabel => $"<{DeviceId + 1}>";

        public DeviceReading()
        {
        }

        public List<Reading> Readings = new List<Reading>();

        private Reading reading;

        public Reading Reading
        {
            get
            {
                if(Readings.LastOrDefault() != null)
                {
                    reading = Readings.LastOrDefault();
                    return reading;
                }
                else
                {
                    return reading;
                }
            }
        }

    }
}
