using ArbitrageDesktopApp.EventArgsModels;
using ArbitrageDesktopApp.Models;
using ArbitrageDesktopApp.ViewModels.Explorers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MenuControlViewModel _menuControlViewModel;
        public MenuControlViewModel MenuControlViewModel
        {
            get => _menuControlViewModel;
            set => SetProperty(ref _menuControlViewModel, value);
        }

        private ExplorerViewModelBase _currentExplorer;
        public ExplorerViewModelBase CurrentExplorer
        {
            get => _currentExplorer;
            set => SetProperty(ref _currentExplorer, value);
        }

        public MainWindowViewModel()
        {
            MenuControlViewModel = new MenuControlViewModel();
            MenuControlViewModel.MenuItemSelectionChanged += MenuControlViewModel_MenuItemSelectionChanged;
        }

        private void MenuControlViewModel_MenuItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.NewSelectedItem is MenuItem menuItem)
            {
                CurrentExplorer = menuItem.ExplorerViewModel;
            }
        }
    }
}
