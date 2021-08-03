using GraphPlotting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

namespace GraphPlotting.ViewModel.Helpers
{
    public class DeviceHelper
    {
        private static string buffer = "";

        private static int index = 0;

        public static List<Reading> ReadSerial()
        {
            var dt = DateTime.Now.Ticks / Configuration.Factor;
            var message = SerialPort.ReadExisting();
            var readings = new List<Reading>();
            // Debug.Write(message);
            foreach (var ch in message)
            {
                if (ch == '\n')
                {
                    if (index == 4)
                    {
                        var values = buffer.Split(",");
                        int spo2;
                        int pulse;
                        int wave;
                        int ss;
                        if (values.Length == 5 && int.TryParse(values[4], out spo2) && int.TryParse(values[3], out pulse) && 
                            int.TryParse(values[2], out wave) && int.TryParse(values[1], out ss))
                        {
                            readings.Add(new Reading(values[0], spo2, pulse, wave, ss, dt));
                        }
                    }
                    buffer = "";
                    index = 0;
                }
                else if (ch == ',')
                {
                    index += 1;
                    buffer += ch;
                }
                else
                {
                    buffer += ch;
                }
            }
            return readings;
        }

        public static SerialPort SerialPort { get; set; }

        public static void Connect(Action<List<Reading>> callback)
        {
            SerialPort.Open();
            SerialPort.DataReceived += new SerialDataReceivedEventHandler((s, e) =>
            {
                var readings = DeviceHelper.ReadSerial();
                callback(readings);
            });
        } 

        public static void Disconnect()
        {
            if (SerialPort?.IsOpen??false)
            {
                SerialPort.Close();
            }
            SerialPort = null;
        }

        public static List<string> GetSerialPorts()
        {
            List<string> ports = new List<string>();
            foreach (string s in SerialPort.GetPortNames())
            {
                ports.Add(s);
            }
            return ports;
        }
    }
}
