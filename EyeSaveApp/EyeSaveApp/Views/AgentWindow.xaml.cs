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
    }
}
