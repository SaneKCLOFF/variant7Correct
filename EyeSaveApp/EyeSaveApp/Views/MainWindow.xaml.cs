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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
        }

        private void btnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            new AgentWindow(null).ShowDialog();
        }

        private void DisplayListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedAgent != null)
                new AgentWindow(_viewModel.SelectedAgent.Id).ShowDialog();
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPage.num == 1)
                return;
            _viewModel.SelectedPage = _viewModel.Pages[_viewModel.SelectedPage.num-2];
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPage.num == _viewModel.Pages.Count)
                return;
            _viewModel.SelectedPage = _viewModel.Pages[_viewModel.SelectedPage.num];
        }
    }
}