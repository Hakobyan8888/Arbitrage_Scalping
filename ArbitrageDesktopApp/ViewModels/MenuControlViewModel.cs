using ArbitrageDesktopApp.Models;
using ArbitrageDesktopApp.ViewModels.Explorers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitrageDesktopApp.EventArgsModels;

namespace ArbitrageDesktopApp.ViewModels
{
    public class MenuControlViewModel : ViewModelBase
    {
        public event EventHandler<SelectionChangedEventArgs> MenuItemSelectionChanged;

        private ObservableCollection<MenuItem> _menuItems;
        private MenuItem _selectedMenuItem;
        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        public MenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                SetProperty(ref _selectedMenuItem, value);
                MenuItemSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(_selectedMenuItem));
            }
        }

        public MenuControlViewModel()
        {
            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    MenuName = "Balances",
                    ExplorerViewModel = new BalancesExplorerViewModel(),
                },
                new MenuItem
                {
                    MenuName = "Scalping Pairs",
                    ExplorerViewModel = new CurrencyPairsExplorerViewModel(),
                },
                new MenuItem
                {
                    MenuName = "Orders Status",
                    ExplorerViewModel = new OrdersStatusExplorerViewModel(),
                },
            };
        }


    }
}
