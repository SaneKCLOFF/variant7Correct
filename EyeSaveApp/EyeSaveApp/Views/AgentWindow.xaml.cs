using EyeSaveApp.ViewModels;
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
using System.Windows.Shapes;

namespace EyeSaveApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : Window
    {
        private AgentWindowViewModel _viewModel;
        public AgentWindow(int? agentId)
        {
            InitializeComponent();
            _viewModel = new AgentWindowViewModel(agentId);
            DataContext=_viewModel;
        }

        private void btnDeleteProductSale_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действиетльно хотите удалить это?","Подтверждение!",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            _viewModel.DeletSelectedProductSale();
        }

        private void btnAddProductSale_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedProduct!=null)
                _viewModel.AddProductSale();
        }

        private void btnDeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteAgent();
            Close();
        }
    }
}