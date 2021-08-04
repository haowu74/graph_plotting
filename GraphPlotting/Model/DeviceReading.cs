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
            Reading = new Reading($"", 0, 0, 0, 0);
        }

        public List<Reading> Readings = new List<Reading>();

        public Reading Reading { get; set; } 

    }
}
