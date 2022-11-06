using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EyeSaveApp.Models.Entities
{
    public partial class Agent
    {
        public Agent()
        {
            AgentPriorityHistories = new HashSet<AgentPriorityHistory>();
            ProductSales = new HashSet<ProductSale>();
            Shops = new HashSet<Shop>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AgentTypeId { get; set; }
        public string? Address { get; set; }
        public string Inn { get; set; } = null!;
        public string? Kpp { get; set; }
        public string? DirectorName { get; set; }
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public int Priority { get; set; }

        public virtual AgentType AgentType { get; set; } = null!;
        public virtual ICollection<AgentPriorityHistory> AgentPriorityHistories { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }

        [NotMapped]
        public string CorrectLogoPath
        {
            get => (Logo == null | Logo == string.Empty) ? @"\Resources\picture.png" : $@"\Resources{Logo}";
        }
        [NotMapped]
        public int SalesPerYear
        {
            get
            {
                var nowDate =DateTime.Now.Year;
                var diapasonDate = nowDate - 5;
                return ProductSales.Where(ps => (nowDate >= ps.SaleDate.Year)&&(ps.SaleDate.Year>=diapasonDate)).Count();
            }
        }
        [NotMapped]
        public string FullTitle
        {
            get => $"{AgentType.Title} | {Title}";
        }
        [NotMapped]
        public int Discount
        {
            get 
            {
                var discount = 0;
                decimal summ = 0;

                foreach (var item in ProductSales)
                {
                    summ += item.ProductCount * item.Product.MinCostForAgent;
                }

                if (summ < 10000)
                    discount = 0;
                else if (summ < 50000)
                    discount = 5;
                else if (summ < 150000)
                    discount = 10;
                else if (summ < 500000)
                    discount = 20;
                else
                    discount = 25;
                return discount;
            }
        }
    }
}
