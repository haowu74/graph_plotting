using GraphPlotting.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class ConnectCommand : ICommand
    {
        public PlotsVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ConnectCommand(PlotsVM vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            // Check if any COM port is selected.
            return !string.IsNullOrEmpty(ViewModel.SelectedSerialPort);
        }

        public void Execute(object parameter)
        {
            bool isConnected = (bool)parameter;
            if (string.IsNullOrEmpty(ViewModel.SelectedSerialPort))
            {
                return;
            }
            if (isConnected)
            {
                if (Disconnect())
                {
                    ViewModel.IsConnected = false;
                    
                }
            }
            else
            {
                if (Connect())
                {
                    ViewModel.IsConnected = true;
                    
                }
            }
        }

        private bool Connect()
        {
            DeviceHelper.SerialPort = new SerialPort(ViewModel.SelectedSerialPort, 115200, Parity.None, 8, StopBits.One);
            DeviceHelper.Connect(ViewModel.DispatchReadings);
            return true;
        }

        private bool Disconnect()
        {
            DeviceHelper.Disconnect();
            return true;
        }
    }
}
