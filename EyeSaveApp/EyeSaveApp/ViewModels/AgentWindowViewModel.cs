using EyeSaveApp.Models;
using EyeSaveApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EyeSaveApp.ViewModels
{
    public class AgentWindowViewModel : ViewModelBase
    {
        private Agent _currentAgent;
        private List<AgentType> _agentTypes;
        private ProductSale _selectedProductSale;
        private string _searchValue;
        private List<Product> _products;
        private List<Product> _displayingProducts;
        private Product _selectedProduct;
        private string _productSaleCount;

        private bool _isNew;
        public List<AgentType> AgentTypes { get => _agentTypes; set => Set(ref _agentTypes, value, nameof(AgentTypes)); }
        public Agent CurrentAgent 
        { 
            get => _currentAgent;
            set => Set(ref _currentAgent, value, nameof(CurrentAgent));
        }
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
        public Product SelectedProduct { get => _selectedProduct; set => Set(ref _selectedProduct, value, nameof(SelectedProduct)); }
        public string ProductSaleCount { get => _productSaleCount; set => Set(ref _productSaleCount, value, nameof(ProductSaleCount)); }

        public AgentWindowViewModel(int? agentId)
        {
            using (ApplicationDbContext context = new())
            {
                AgentTypes = context.AgentTypes.ToList();
                Products=context.Products.ToList();
            }
            if (agentId==null)
            {
                _isNew = true;
                CurrentAgent = new Agent();
            }
            else
            CurrentAgent = GetAgent((int)agentId);
            DisplayProducts();
        }
        public void DeletSelectedProductSale()
        {
            using (ApplicationDbContext context = new())
            {
                context.ProductSales.Remove(SelectedProductSale);
                context.SaveChanges();
            }
            SelectedProductSale = null;
            CurrentAgent = GetAgent(CurrentAgent.Id);
        }
        public void AddProductSale()
        {
            using (ApplicationDbContext context=new())
            {
                ProductSale newProductSale = new()
                {
                    AgentId = CurrentAgent.Id,
                    ProductId = SelectedProduct.Id,
                    SaleDate = DateTime.Now,
                    ProductCount = Convert.ToInt32(ProductSaleCount)
                };
                context.ProductSales.Add(newProductSale);
                context.SaveChanges();
            }
            CurrentAgent = GetAgent(CurrentAgent.Id);
        }
        public void AddAgent()
        {
            using (ApplicationDbContext context=new())
            {
                CurrentAgent.AgentTypeId = CurrentAgent.AgentType.Id;
                CurrentAgent.AgentType = null;
                context.Agents.Add(CurrentAgent);
                context.SaveChanges();
            }
        }
        public void UpdateAgent()
        {
            using (ApplicationDbContext context = new()) 
            {
                context.Agents.Update(CurrentAgent);
                context.SaveChanges();
            }
        }
        public void DeleteAgent()
        {
            if (CurrentAgent!=null)
            {
                using (ApplicationDbContext context = new())
                {
                    context.Agents.Remove(CurrentAgent);
                    context.SaveChanges();
                }
                CurrentAgent = null;
            }
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
