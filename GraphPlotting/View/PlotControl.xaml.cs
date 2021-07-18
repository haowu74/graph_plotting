using GraphPlotting.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace GraphPlotting.View
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : UserControl, INotifyPropertyChanged
    {
        public int Id
        {
            get 
            { 
                return (int)this.GetValue(IdProperty); 
            }
            set 
            { 
                this.SetValue(IdProperty, value); 
            }
        }

        public PlotVM Plot { get; set; }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int),
            typeof(PlotControl));

        public PlotControl()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                Plot = new PlotVM(Id);
                _timer = new Timer(state =>
                {
                    OnPropertyChanged("Plot");
                }, null, 1000, 1000);
            };
        }

        private Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
