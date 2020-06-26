using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.ItemsContainer;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.ViewModel
{
    public class MPSTLViewModel : ViewModelBase
    {

        public MPSTLViewModel()
        {
            OnDataLoad();
            selectedTLType = TLRunningStatus.等待;
        }

        private RelayCommand feedingCommand;
        private string filterItemString; 
        private string selectedWorkShop;

        public List<MPSTLItem> MPSTLItemsView { get; set; }


        public MPSTLItem SelectedMPSTLItem { get; set; } = new MPSTLItem();

        //public Array StatusSource => System.Enum.GetValues(typeof(FeedingType));    

        public string FilterItemString { get => filterItemString; set { filterItemString = value; OnQueryChanged(); } }

        public string SelectedRclLinev
        {
            get => selectedRclLine;
            set
            {
                selectedRclLine = value; OnQueryChanged();
                selectedWorkShop = string.Empty;
                RaisePropertyChanged("QueryWorkShopView");
                RaisePropertyChanged("SelectedOrderDate");

            }
        }
        private string selectedRclLine;


        private TLRunningStatus selectedTLType;

        public TLRunningStatus SelectedTLType
        {
            get =>selectedTLType;
            set { selectedTLType = value;
                OnQueryChanged();
            }
        }


        private void OnQueryChanged()
        {

            using (MyKingdeeDBContext _context = new MyKingdeeDBContext())
            {
                MPSTLItemsView = new TLScheduleRepository().GetALL().Where(x => x.RunningStatus ==selectedTLType)
                    .Where(x => string.IsNullOrEmpty(filterItemString) || x.SubFNumber.Contains(filterItemString) || x.CPItem.K3FNumber.Contains(filterItemString)) 
                    .OrderByDescending(b => b.XH).ThenBy(b => b.HandleStatus)
                    .ToList();
            }
            RaisePropertyChanged("MPSTLItemsView");
        }

        private void OnDataLoad()
        {
             
                MPSTLItemsView =new TLScheduleRepository().GetALL().Where(x => x.RunningStatus != TLRunningStatus.完成).OrderBy(b => b.HandleStatus).ThenBy(b => b.XH)
                    .ToList();
            
            RaisePropertyChanged("MPSTLItemsView");
        }

        private RelayCommand outFeedingCommand;

        public RelayCommand OutFeedingCommand
        {
            get
            {
                if (outFeedingCommand == null)
                {
                    outFeedingCommand = new RelayCommand(() =>
                    {
                          OutFeedingCommandExec();
                    }, CanOutFeeding);
                }
                return outFeedingCommand;
            }
        }

        private bool CanOutFeeding()
        {

            return SelectedMPSTLItem == null || (SelectedMPSTLItem.FeedingDate.HasValue && !SelectedMPSTLItem.OutFeedingDate.HasValue);
        }

        /// <summary>
        /// 热处理完成 出料
        /// </summary>
        /// <returns></returns>
        private  void  OutFeedingCommandExec()
        {
            MPSTLContainer.Update(SelectedMPSTLItem);
        }

        public RelayCommand FeedingCommand
        {
            get
            {
                if (feedingCommand == null)
                {
                    feedingCommand = new RelayCommand(FeedingCommandExec, CanFeeding);
                }
                return feedingCommand;
            }
        }

        private bool CanFeeding() => SelectedMPSTLItem == null || !SelectedMPSTLItem.FeedingDate.HasValue;

        private void FeedingCommandExec()
        {
            SelectedMPSTLItem.FeedingDate = DateTime.Now;
        }



        private RelayCommand uCommand;
        public RelayCommand UCommand
        {
            get
            {
                if (uCommand == null)
                {
                    uCommand = new RelayCommand(uCommandExec, CanuFeeding);
                }
                return uCommand;
            }
        }

        private bool CanuFeeding()
        {
            return true;
        }

        private void uCommandExec()
        {
            //new TLScheduleRepository().UpdateQty(SelectedMPSTLItem);
        }




        private RelayCommand refreshCommand;

        public RelayCommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new RelayCommand(RefreshCommandExec);
                }
                return refreshCommand;
            }
        }
        private void RefreshCommandExec()
        {
            OnQueryChanged();
        }

    }
}
