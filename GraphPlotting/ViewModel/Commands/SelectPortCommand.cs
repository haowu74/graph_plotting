using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class SelectPortCommand : ICommand
    {
        public PlotsVM ViewModel { get; set; }
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string selectedPort = parameter as string;
            ViewModel.SelectedSerialPort = selectedPort;
            var commands = ViewModel.SelectPortCommands.ToList();
            ViewModel.SelectPortCommands.Clear();

            foreach (var command in commands)
            {
                if (command.PortName == selectedPort)
                {
                    command.IsSelected = true;
                }
                else
                {
                    command.IsSelected = false;
                }
                ViewModel.SelectPortCommands.Add(command);

            }
        }

        public SelectPortCommand(PlotsVM vm)
        {
            ViewModel = vm;
        }
    }
}
