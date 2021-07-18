using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.Model
{
    public class DeviceReadings
    {
        private static DeviceReadings _Instance;
        public static DeviceReadings Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new DeviceReadings();
                }
                return _Instance;
            }
        }

        private DeviceReadings()
        {
        }

        public List<Reading> Readings = new List<Reading>();
    }
}
