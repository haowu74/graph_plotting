using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class ConnectCommand : ICommand
    {
        public PlotsVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public ConnectCommand(PlotsVM vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            bool isConnected = (bool)parameter;
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
            return true;
        }

        private bool Disconnect()
        {
            return true;
        }
    }
}
