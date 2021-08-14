using GraphPlotting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class MeanCommand : ICommand
    {
        public PlotsVM ViewModel { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (ViewModel.FilterMode != FilterMode.Mean)
            {
                ViewModel.FilterMode = FilterMode.Mean;
            }
            else
            {
                ViewModel.FilterMode = FilterMode.None;
            }
        }

        public MeanCommand(PlotsVM vm)
        {
            ViewModel = vm;
        }
    }
}
