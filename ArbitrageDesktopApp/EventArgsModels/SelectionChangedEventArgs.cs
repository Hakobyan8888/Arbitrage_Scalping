using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.EventArgsModels
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs()
        {

        }
        public SelectionChangedEventArgs(object newSelectedItem)
        {
            NewSelectedItem = newSelectedItem;
        }
        public SelectionChangedEventArgs(object newSelectedItem, object previousSelectedItem)
        {
            NewSelectedItem = newSelectedItem;
            PreviousSelectedItem = previousSelectedItem;
        }
        public object NewSelectedItem { get; set; }
        public object PreviousSelectedItem { get; set; }
    }
}
