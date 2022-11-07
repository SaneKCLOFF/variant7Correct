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
    public class AgentWindowViewModel : ViewModelBase
    {
        private Agent _agent;
        private List<AgentType> _agentTypes;
        private ProductSale _selectedProductSale;
        private string _searchValue;
        private List<Product> _products;
        private List<Product> _displayingProducts;

        private bool _isNew;
        public List<AgentType> AgentTypes { get => _agentTypes; set => Set(ref _agentTypes, value, nameof(AgentTypes)); }
        public Agent Agent { get => _agent; set => Set(ref _agent, value, nameof(Agent)); }
        public List<Product> Products { get => _products; set => Set(ref _products, value, nameof(Products)); }
        public string SearchValue 
        { 
            get => _searchValue; 
            set
            {
                Set(ref _searchValue, value, nameof(SearchValue));
                DisplayProducts();
            }
        }
        public ProductSale SelectedProductSale { get => _selectedProductSale; set => Set(ref _selectedProductSale,value,nameof(SelectedProductSale)); }
        public List<Product> DisplayingProducts { get => _displayingProducts; set => Set(ref _displayingProducts, value, nameof(DisplayingProducts)); }

        public AgentWindowViewModel(int? agentId)
        {
            using (ApplicationDbContext context = new())
            {
                AgentTypes = context.AgentTypes.ToList();
                _products=context.Products.ToList();
            }
            if (agentId==null)
            {
                _isNew = true;
                Agent = new Agent();
                return;
            }
            Agent = GetAgent((int)agentId);
            DisplayProducts();
        }
        public Agent GetAgent(int agentId)
        {
            using (ApplicationDbContext context = new())
            {
                return context.Agents
                    .Include(at=>at.AgentType)
                    .Include(ps=>ps.ProductSales)
                    .ThenInclude(p=>p.Product)
                    .Single(a=>a.Id==agentId);
            }
        }
        private void DisplayProducts()
        {
            DisplayingProducts = Search(_products);
        }
        public List<Product> Search(List<Product> products)
        {
            if (SearchValue == null || SearchValue == string.Empty)
                return products;
            else
                return products.Where(p => p.Title.ToLower().Contains(SearchValue.ToLower())).ToList();
        }
    }
}
