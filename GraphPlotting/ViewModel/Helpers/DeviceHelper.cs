using GraphPlotting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;

namespace GraphPlotting.ViewModel.Helpers
{
    public class DeviceHelper
    {
        private static string buffer = "";

        private static int index = 0;

        private static string logMessage = "";

        public static List<Reading> ReadSerial()
        {
            var message = SerialPort.ReadExisting();
            var readings = new List<Reading>();
            logMessage += message;
            if (logMessage.Length > 2000)
            {
                LogMessage();
                logMessage = "";
            }

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
                            readings.Add(new Reading(values[0], spo2, pulse, wave, ss));
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

        private static StreamWriter Sw;

        private static void LogMessage()
        {
            Sw.Write(logMessage);
        }

        private static string logFile = @".\log.txt";

        public static void OpenFile()
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
            Sw = File.CreateText(logFile);
        }

        public static void CloseFile()
        {
            Sw.Close();
        }
    }
}
