using GalaSoft.MvvmLight;
using ProduceFeeder.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProduceFeeder.UI.ViewModel
{
    public class CPComponetInventoryViewModel : ViewModelBase
    {

        public CPComponetInventoryViewModel()
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            {
                CPItemslist = _context.K3Items.Where(w => w.FNumber.StartsWith("5.18.") && w.FDeleted == 0).ToList();
            }
        }
        private List<K3Item> CPItemslist;
        private string filterText;

        public ICollectionView CPItemsView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(CPItemslist);
            }
        }

        public string FilterText { get => filterText; set => FilterTextChanged(value); }

        private void FilterTextChanged(string value)
        {
            value = value.ToUpper();
            if (CPItemslist == null) return;
            CPItemsView.Filter = new Predicate<object>(x =>
            {

                var vm = (K3Item)x;
                return vm.FNumber.Contains(value);
            });

            filterText = value;
            CPItemsView?.Refresh();
        }

        private K3Item selectedK3Item;

        public K3Item SelectedK3Item
        {
            get { return selectedK3Item; }
            set
            {
                selectedK3Item = value;
                if (value != null)
                { 
                    OnK3ItemSelected(value);
                }
            }
        }

        private void OnK3ItemSelected(K3Item value)
        {
            //MPSICMO _item = new MPSICMO { K3ItemID = value.ID };
            ////_item.BOMDecomposeWithNoProcess();
            //K3ItemComponents = _item.ComponentItems;
            ////K3ItemComponents.ForEach(f => f.GetInventory());    
            //RaisePropertyChanged("K3ItemComponents");
        }

        public List<BOMItemBase> K3ItemComponents { get; set; }
    }
}
