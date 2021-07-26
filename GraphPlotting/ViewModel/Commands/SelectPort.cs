using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GraphPlotting.ViewModel.Commands
{
    public class SelectPort
    {
        public string PortName { get; set; }

        public ICommand SelectPortCommand { get; set; }

        public bool IsSelected { get; set; }
    }
}
