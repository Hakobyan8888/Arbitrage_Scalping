using ArbitrageDesktopApp.ViewModels.Explorers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.Models
{
    public class MenuItem : BindableBase
    {
        public string MenuName { get; set; }

        public ExplorerViewModelBase ExplorerViewModel { get; set; }
    }
}
