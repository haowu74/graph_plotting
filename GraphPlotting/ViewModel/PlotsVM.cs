using GraphPlotting.Model;
using GraphPlotting.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotting.ViewModel
{
    public class PlotsVM
    {
        public PlotsVM()
        {
            DeviceHelper.Run();
            DeviceReadings = new List<DeviceReading>();
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 0,
                Readings = new List<Reading>() { new Reading(0, 12, 0, 0, 0, DateTime.Now) }
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 1,
                Readings = new List<Reading>() { new Reading(1, 34, 0, 0, 0, DateTime.Now) }
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 2,
                Readings = new List<Reading>() { new Reading(2, 56, 0, 0, 0, DateTime.Now) }
            });
            DeviceReadings.Add(new DeviceReading()
            {
                DeviceId = 3,
                Readings = new List<Reading>() { new Reading(3, 78, 0, 0, 0, DateTime.Now) }
            });
        }

        public List<DeviceReading> DeviceReadings { get; set; }
    }
}
