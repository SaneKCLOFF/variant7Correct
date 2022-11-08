using EyeSaveApp.Models;
using EyeSaveApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EyeSaveApp.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        #region Main Code
        //private List<Agent> _agents;
        private List<Agent> _displayingAgents;
        private string _sortValue;
        private string _searchValue;
        private string _filterValue;

        private Agent _selectedAgent;

        public List<Agent> DisplayingAgents
        {
            get => _displayingAgents;
            set => Set(ref _displayingAgents, value, nameof(DisplayingAgents));
        }
        public string FilterValue 
        { 
            get => _filterValue; 
            set
            {
                if (Set(ref _filterValue, value, nameof(FilterValue)))
                    DisplayAgents();
            } 
        }
        public string SearchValue 
        { 
            get => _searchValue; 
            set
            {
                if (Set(ref _searchValue, value, nameof(SearchValue)))
                    DisplayAgents();
            }
        }
        public string SortValue 
        { 
            get => _sortValue;
            set 
            {
                if (Set(ref _sortValue, value, nameof(SortValue)))
                    DisplayAgents();
            }
        }
        public List<string> ValuesToSort { get; } = new List<string>
        {
            "Без сортировки",
            "Наименование (возр)",
            "Наименование (уб)",
            "Размер скидки (возр)",
            "Размер скидки (уб)",
            "Приоритет (возр)",
            "Приоритет (уб)"
        };

        public List<string> ValuesToFilter { get; } = new List<string>
        {
            "Все типы"
        };
        public Agent SelectedAgent 
        { 
            get => _selectedAgent;
            set => Set(ref _selectedAgent, value, nameof(SelectedAgent));
        }

        public MainWindowViewModel()
        {
            using (ApplicationDbContext context = new())
            {
                ValuesToFilter.AddRange(context.AgentTypes.Select(a => a.Title));

                //_agents = UpdateAgents();
            }
            _displayingAgents= new List<Agent>(UpdateAgents());
            _filterValue = ValuesToFilter[0];
            _sortValue = ValuesToSort[0];
            _pages = GetPages(_displayingAgents.Count);
            SelectedPage = _pages[0];

        }        
        public List<Agent> UpdateAgents()
        {
            using (ApplicationDbContext context= new())
            {
                return context.Agents.AsNoTracking()
                    .Include(at => at.AgentType)
                    .Include(ps => ps.ProductSales)
                    .ThenInclude(p => p.Product)
                    .OrderBy(a => a.Id)
                    .ToList();
            }
        }
        private void DisplayAgents()
        {
            var list = Sort(Search(Filter(UpdateAgents())));
            Pages = GetPages(list.Count());
            var pageNum = SelectedPage == null
                ? 1
                : SelectedPage.num;
            DisplayingAgents = Paging(list,pageNum).ToList();

            SelectedPage ??= Pages[0];
        }

        private IEnumerable<Agent> Sort(IEnumerable<Agent> agents)
        {
            var sort = SortValue;

            if (sort == ValuesToSort[1])
                return agents.OrderBy(a => a.Title);
            else if (sort == ValuesToSort[2])
                return agents.OrderByDescending(a => a.Title);
            else if (sort == ValuesToSort[3])
                return agents.OrderBy(a => a.Discount);
            else if (sort == ValuesToSort[4])
                return agents.OrderByDescending(a => a.Discount);
            else if (sort == ValuesToSort[5])
                return agents.OrderBy(a => a.Priority);
            else if (sort == ValuesToSort[6])
                return agents.OrderByDescending(a => a.Priority);
            else
                return agents;
        }
        private IEnumerable<Agent> Filter(IEnumerable<Agent> agents)
        {
            var filter = FilterValue;
            if (filter == ValuesToFilter[0])
                return agents;
            else
                return agents.Where(a=>a.AgentType.Title==filter);
        }
        private IEnumerable<Agent> Search(IEnumerable<Agent> agents)
        {
            var search = SearchValue;
            if (search == null || search == string.Empty)
                return agents;
            else
                return agents.Where(a=>a.Title.ToLower().Contains(search.ToLower()) || a.Phone.ToLower().Contains(search.ToLower()) || a.Email.ToLower().Contains(search.ToLower()));
        }
        #endregion

        #region Pagging Code
        public record Page(int num);
        private const int PageSize = 10;
        private List<Page> _pages;
        private Page _selectedPage;

        public List<Page> Pages 
        { 
            get => _pages;
            set => Set(ref _pages, value, nameof(Pages));
        }
        public Page SelectedPage 
        { 
            get => _selectedPage;
            set
            {
                if (Set(ref _selectedPage, value, nameof(SelectedPage)))
                    DisplayAgents();
            }
        }

        private List<Page> GetPages(int itemsCount)
        {
            double pageCount = Math.Ceiling((double)itemsCount/PageSize);
            var list = new List<Page>((int)pageCount);
            list.Add(new Page(1));
            for (int i = 2; i <= pageCount; i++)
            {
                list.Add(new Page(i));
            }
            return list;
        }

        private IEnumerable<Agent> Paging(IEnumerable<Agent> agents, int pageNum)
        {
            if (pageNum > 0)
                agents = agents.Skip((pageNum-1)*PageSize);

            return agents.Take(PageSize);
        }
        #endregion
    }
}
