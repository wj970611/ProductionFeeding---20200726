using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProduceFeeder.UI.ViewModel
{
    public class MPSYPFeedingViewModel : ViewModelBase
    {



        public string WindowTitle { get; set; } = "投料预排表";

        public MPSYPFeedingViewModel()
        {
            DataLoad();
        }

        private List<MPSYPItem> lst = new List<MPSYPItem>();

        public List<MPSYPItemRCLPrev> MPSYPItemYTView
        {
            get
            {
                if (selectedMpsYpitem == null) return null;
                return SelectedMPSYPItem.GetYT();
            }
        }
        public ObservableCollection<MPSYPItem> MPSYPItemDataView => new ObservableCollection<MPSYPItem>(lst);


        private MPSYPItem selectedMpsYpitem;

        public MPSYPItem SelectedMPSYPItem
        {

            get { return selectedMpsYpitem; }
            set
            {
                if (value != null)
                {
                    selectedMpsYpitem = value;
                    ProcessedItemsView = SelectedMPSYPItem?.CPItem.PrevRCLProcessItems;
                    RaisePropertyChanged("SelectedMPSYPItem");
                    RaisePropertyChanged("MPSYPItemYTView");

                }
            }

        }

        private List<BOMProcessedItem> processeditem = new List<BOMProcessedItem>();
        public List<BOMProcessedItem> ProcessedItemsView
        {
            get
            {
                return processeditem;
            }
            set
            {
                processeditem = value;
                RaisePropertyChanged("ProcessedItemsView");
            }
        }



        #region Command

        /// <summary>
        /// 分投
        /// </summary>
        private RelayCommand ftlcommand;

        public RelayCommand FTLCommand
        {
            get
            {
                if (ftlcommand == null)
                {
                    ftlcommand = new RelayCommand(FTLCommandExec, CanFTL);
                }
                return ftlcommand;
            }
        }

        private void FTLCommandExec()
        {
            selectedMpsYpitem.CPItem.PrevRCLProcessItems.ForEach(x =>
            {
                new ICMO
                {
                    FauxQty = selectedMpsYpitem.OnePlanQTY,
                    FBomInterID = selectedMpsYpitem.CPItem.BOMId,
                    FWorkShop = selectedMpsYpitem.CPItem.DepId,
                    FPlanCommitDate = DateTime.Now.Date,
                    FPlanFinishDate = DateTime.Now.Date,
                    FItemId = x.K3ItemID,
                    QJId = selectedMpsYpitem.QJId,
                    WorkId = 55,UnitID=305
                }.InsertICMO();

            });
            processeditem.Clear();
            selectedMpsYpitem.RunningStatus = EnumRunningStatus.等待同步;
            using (MyKingdeeDBContext _context = new MyKingdeeDBContext())
            {
                var item = _context.MPSYPFeedings.Find(selectedMpsYpitem.ID);
                item.RunningStatus = EnumRunningStatus.等待同步;
                _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        private bool CanFTL()
        {
            //大小圈数量要一致
            return selectedMpsYpitem != null && selectedMpsYpitem.RunningStatus == EnumRunningStatus.工序异步 && selectedMpsYpitem.CanYTL;
        }


        private RelayCommand tlcommand;

        public RelayCommand TLCommand
        {
            get
            {
                if (tlcommand == null)
                {
                    tlcommand = new RelayCommand(TLCommandExec, CanTL);
                }
                return tlcommand;
            }
        }

        private void TLCommandExec()
        {
            Messenger.Default.Send<MPSYPItem>(selectedMpsYpitem, "SelectedYPToTLView");
        }

        private bool CanTL()
        {
            //大小圈数量要一致
            return selectedMpsYpitem != null;
        }

        private RelayCommand messageCommand;

        public RelayCommand MessageCommand
        {
            get
            {
                if (messageCommand == null)
                {
                    messageCommand = new RelayCommand(MessageCommandExec);
                }
                return messageCommand;
            }
        }

        private void MessageCommandExec()
        {

            //NotificationMessageAction<MessageBoxResult> msg = new NotificationMessageAction<MessageBoxResult>("主页面测试", (ret) =>
            //{
            //    if (ret == MessageBoxResult.OK)
            //    {
            //        MessageBox.Show("弹窗点击了确认。继续执行。");
            //    }
            //}
            //        );

            //Messenger.Default.Send<NotificationMessageAction<MessageBoxResult>>(msg);
            //Messenger.Default.Send("Start", "StartMessager");
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
            DataLoad();
        }

        private void DataLoad()
        {
            lst = new MPSYPItemRepository().GetALL().Where(x => x.RunningStatus != EnumRunningStatus.完成).ToList();
            //这里要提供异步方法
            lst?.ForEach(async x => await x.CPItem.OnItemIDChangedAsync());
            RaisePropertyChanged("ICMOFeedingView");
        }

        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteCommandExec, CanDelete);
                }
                return deleteCommand;
            }
        }

        private bool CanDelete()
        {
            return SelectedMPSYPItem != null;
        }

        private void DeleteCommandExec()
        {
            NotificationMessageAction<MessageBoxResult> msg = new NotificationMessageAction<MessageBoxResult>("", (ret) =>
            {
                if (ret == MessageBoxResult.OK)
                {
                    lst.Remove(SelectedMPSYPItem);
                    new MPSYPItemRepository().Delete(SelectedMPSYPItem); RaisePropertyChanged("ICMOFeedingView");
                }
            }
            );

            Messenger.Default.Send<NotificationMessageAction<MessageBoxResult>>(msg);
        }



        private RelayCommand saveYPCommand;

        public RelayCommand SaveYPCommand
        {
            get
            {
                if (saveYPCommand == null)
                {
                    saveYPCommand = new RelayCommand(SaveYPCommandExec);
                }
                return saveYPCommand;
            }
        }
        private void SaveYPCommandExec()
        {
            selectedMpsYpitem.ExecTL();
            selectedMpsYpitem.CanExecTL();
        }

        private RelayCommand copyItemFNumberCommand;

        public RelayCommand CopyItemFNumberCommand
        {
            get
            {
                if (copyItemFNumberCommand == null)
                {
                    copyItemFNumberCommand = new RelayCommand(CopyItemFNumberCommandExec, CanContextMenu);
                }
                return copyItemFNumberCommand;
            }
        }

        private void CopyItemFNumberCommandExec()
        {
            Clipboard.SetText(selectedMpsYpitem.CPItem.K3FNumber);
        }


        private RelayCommand copyItemFNameCommand;

        public RelayCommand CopyItemFNameCommand
        {
            get
            {
                if (copyItemFNameCommand == null)
                {
                    copyItemFNameCommand = new RelayCommand(CopyItemFNameCommandExec, CanContextMenu);
                }
                return copyItemFNameCommand;
            }
        }

        private bool CanContextMenu()
        {
            return selectedMpsYpitem != null;
        }

        private void CopyItemFNameCommandExec()
        {
            Clipboard.SetText(selectedMpsYpitem.CPItem.K3FModel);
        }



        private RelayCommand copyItemFIDCommand;
        private string selectedWorkShop;

        public RelayCommand CopyItemFIDCommand
        {
            get
            {
                if (copyItemFIDCommand == null)
                {
                    copyItemFIDCommand = new RelayCommand(CopyItemFIDCommandExec, CanContextMenu);
                }
                return copyItemFIDCommand;
            }
        }

        private void CopyItemFIDCommandExec()
        {
            Clipboard.SetText(selectedMpsYpitem.CPItem.K3ItemID.ToString());
        }

        #endregion
    }
}
