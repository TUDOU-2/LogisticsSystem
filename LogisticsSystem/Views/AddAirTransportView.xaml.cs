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
    /// AddAirTransportView.xaml 的交互逻辑
    /// </summary>
    public partial class AddAirTransportView : Window
    {
        public AddAirTransportView(AddAirTransportViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void GridVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            if (grid1.Visibility == Visibility.Visible)
            {
                grid1.Visibility = Visibility.Collapsed;
                grid2.Visibility = Visibility.Visible;
            }
            else
            {
                grid1.Visibility = Visibility.Visible;
                grid2.Visibility = Visibility.Collapsed;
            }
        }
    }
}
