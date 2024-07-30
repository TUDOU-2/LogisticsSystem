using LogisticsSystem.ViewModels;
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

namespace LogisticsSystem.Views
{
    /// <summary>
    /// ExpressTransportView.xaml 的交互逻辑
    /// </summary>
    public partial class ExpressTransportView : UserControl
    {
        public ExpressTransportView(ExpressTransportViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
