using GraphPlotting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl
    {
        private PlotVM viewModel;

        public object Id 
        { 
            get
            {
                return viewModel.Id;
            }
            set 
            {
                viewModel.Id = value;
            } 
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string),
            typeof(PlotControl), new PropertyMetadata(""));

        public PlotControl()
        {
            viewModel = new PlotVM();
            InitializeComponent();
        }
    }
}
