using GraphPlotting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GraphPlotting.ViewModel.Helpers
{
    public class DeviceHelper
    {
        private static bool isRunning;

        private static Action Process = () =>
        {
            while(isRunning)
            {
                // DeviceReadings.Instance.Readings.AddRange(ReadSerial());
                Thread.Sleep(1000);
            }
        };

        public static List<Reading> ReadSerial()
        {
            DateTime dt = DateTime.Now;
            Random rand = new Random();
            var readings = new List<Reading>();
            for (int id = 1; id <= 4; id++)
            {
                var spo2 = rand.Next(100);
                var pulse = rand.Next(100);
                var wave = rand.Next(100);
                var ss = rand.Next(50);
                readings.Add(new Reading(id, spo2, pulse, wave, ss, dt));
            }
            return readings;
        }

        public static void Run()
        {
            isRunning = true;
            Task.Run(Process);
        } 

        public static void Stop()
        {
            isRunning = false;
        }
    }
}
