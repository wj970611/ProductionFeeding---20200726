using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.ViewModel
{
    public class YPToScheduleViewModel : ViewModelBase
    {



        /// <summary>
        /// 保持的排料
        /// </summary>
        public ObservableCollection<MPSTLItem> SavedFeedingItemsView => new ObservableCollection<MPSTLItem>(savedtlitems);
        private List<MPSTLItem> savedtlitems = new List<MPSTLItem>();
        public List<string> GetRclLine()
        {
            return new List<string> { "新线1", "老线2" };
        }

        public MPSItemBase ypItem { get; set; }

        public MPSItemBase YpItemImport
        {
            get => ypItem; set
            {
                ypItem = value;
                var list = new List<MPSTLItem>();
                ypItem.CPItem.PrevRCLProcessItem.ForEach(f =>
                {
                    var a = new MPSTLItem
                    {
                        CPItem = ypItem.CPItem,
                        PlanStamp = ypItem.PlanStamp,
                        QJId = ypItem.QJId,
                        OnePlanQTY = ypItem.OnePlanQTY,
                        SubFNumber = f.K3FNumber,
                        SubFItemId = f.K3ItemID,
                        SubFName = f.K3FName,
                        SubFModel = f.K3FModel,  
                        DXQ = f.DXQ,
                        Qty = ypItem.OnePlanQTY,
                    };
                //a.GetInventory();

                list.Add(a);
            });
            FeedingItemsView = list;
        }
    }

    /// <summary>
    /// 待投物料
    /// </summary>
    public List<MPSTLItem> FeedingItemsView
    {
        get => feedingItemsView; set
        {
            feedingItemsView = value;
            RaisePropertyChanged("FeedingItemsView");
        }
    }

    private MPSTLItem selectedFeedingItem;
    public MPSTLItem SelectedFeedingItem
    {
        get => selectedFeedingItem;
        set
        {
            selectedFeedingItem = value;
            if (value != null)
            {

                SelectedItemICInventories = ICInventory.GetICInventories(selectedFeedingItem.SubFItemId);
                SelectedItemPOInventories = POInventory.GetPOInventories(selectedFeedingItem.SubFItemId);
                RaisePropertyChanged("SelectedItemICInventories");
                RaisePropertyChanged("SelectedItemPOInventories");

                switch (selectedFeedingItem.HandleStatus)
                {
                    case TLhandleStatus.Normal:
                        IconPath = "image/flag_red.png";
                        break;
                    case TLhandleStatus.Urgent:
                        IconPath = "image/flag_blue.png";
                        break;
                }
            }
        }
    }

    public List<ICInventory> SelectedItemICInventories { get; set; }
    public List<POInventory> SelectedItemPOInventories { get; set; }
    #region Command


    private RelayCommand saveCommand;

    public RelayCommand SaveCommand
    {
        get
        {
            if (saveCommand == null)
            {
                saveCommand = new RelayCommand(SaveCommandExec);
            }
            return saveCommand;
        }
    }
    private void SaveCommandExec()
    {
        foreach (var item in FeedingItemsView)
        {
            new Repository.TLScheduleRepository().Insert(item);
            savedtlitems.Add(item);
        }

        FeedingItemsView = new List<MPSTLItem>();

        RaisePropertyChanged("FeedingItemsView");
        RaisePropertyChanged("SavedFeedingItemsView");
    }
    private RelayCommand redFlagCommand;
    private string iconPath = "image/flag_red.png";
    private List<MPSTLItem> feedingItemsView = new List<MPSTLItem>();



        private RelayCommand mpsTLWindowShow;

        public RelayCommand MpsTLWindowShow
        {
            get
            {
                if (mpsTLWindowShow == null)
                {
                    return mpsTLWindowShow = new RelayCommand(mpsTLWindowShowExec);
                }
                return mpsTLWindowShow;
            }
        }
        private void mpsTLWindowShowExec()
        {
            Messenger.Default.Send<string>("null", "MpsTLWindowShow");
        }

        public RelayCommand RedFlagCommand
    {
        get
        {
            if (redFlagCommand == null)
            {
                redFlagCommand = new RelayCommand(RedFlagCommandExec, CanRedFlag);
            }
            return redFlagCommand;
        }
    }

    public string IconPath { get => iconPath; private set { iconPath = value; RaisePropertyChanged("IconPath"); } }

    private bool CanRedFlag()
    {
        return selectedFeedingItem != null;
    }

    private void RedFlagCommandExec()
    {
        if (selectedFeedingItem.HandleStatus == TLhandleStatus.Normal)
        {

            selectedFeedingItem.HandleStatus = TLhandleStatus.Urgent;
        }
        else if (selectedFeedingItem.HandleStatus == TLhandleStatus.Urgent)
        {

            selectedFeedingItem.HandleStatus = TLhandleStatus.Normal;
        }
    }

    #endregion

}
}

