using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class ClearCommand : ICommand
    {
        public PlotsVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ClearCommand(PlotsVM vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !ViewModel.IsConnected;
        }

        public void Execute(object parameter)
        {
            ViewModel.ClearPlots();
        }
    }
}
