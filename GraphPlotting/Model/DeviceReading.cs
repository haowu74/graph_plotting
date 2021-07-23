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

        public DeviceReading()
        {
        }

        public List<Reading> Readings = new List<Reading>();

        public Reading Reading => Readings.LastOrDefault();
    }
}
