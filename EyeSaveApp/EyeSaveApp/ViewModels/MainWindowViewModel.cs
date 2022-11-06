using EyeSaveApp.Models;
using EyeSaveApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeSaveApp.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        private List<Agent> _agents;
        private List<Agent> _displayingAgents;

        public MainWindowViewModel()
        {
            using (ApplicationDbContext context = new())
            {
                _agents = context.Agents
                    .Include(at => at.AgentType)
                    .Include(ps => ps.ProductSales)
                    .ThenInclude(p => p.Product)
                    .ToList();
            }
            DisplayingAgents= new List<Agent>(_agents);
        }

        public List<Agent> DisplayingAgents 
        { 
            get => _displayingAgents;
            set => Set(ref _displayingAgents, value, nameof(DisplayingAgents));
        }
    }
}
