using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.BOMItemQuery;
using ProduceFeeder.UI.Models.ItemsContainer;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProduceFeeder.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MpsSelecteViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        ///  
        public MpsSelecteViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            using (K3BhDBContext _context = new K3BhDBContext())
            {

                var qj = _context.QJItems.Where(q => q.StartDate.Year == DateTime.Now.Year && q.StartDate.Month == DateTime.Now.Month).FirstOrDefault();
                QJItemView = _context.QJItems.Where(q => q.FItemId > qj.FItemId - 3 && q.FItemId < qj.FItemId + 3).ToList();


            };
            //齐套  

        }

        private async Task MPSDataLoad(QJItem qJItem)
        {
            using (K3BhDBContext k3DBContext = new K3BhDBContext())
            {
                LoadWaitvisibility = Visibility.Visible;
               
                PlanProItemView = await new K3MPSRepository().GetAll()
                    .Where(x => x.QJId == qJItem.FItemId)
                    .Where(x=>x.K3Dep.FName !="制造二科")  
                    .GroupBy(g => new
                    {
                        g.FItemID,
                        FNumber = g.K3Item.FNumber,
                        CPCJ = g.K3Dep,
                        FItem = g.K3Item
                    })
                    .Select(s => new MPSPlanItem
                    {
                        PlanTotalQty = (int)s.Sum(l => l.FQty),
                        PlanCount = s.Count(),
                        CPItem = new K3CPItemBase
                        {
                            K3FNumber = s.Key.FItem.FNumber,
                            K3FModel = s.Key.FItem.FModel,
                            DepId = s.Key.CPCJ.Id,
                            DepFName = s.Key.CPCJ.FName,
                            K3ItemID = s.Key.FItemID
                        },
                        QJId = SelectedQJItem.FItemId
                    }).ToListAsync();
            }

            PlanProItemView.ForEach(async x => await x.GetQtyAsync(qJItem));
            WorkShopView = PlanProItemView.Select(s => s.CPItem?.DepFName).DefaultIfEmpty().Distinct().ToList();
            FilterPlanQty = PlanProItemView.Select(s => s.PlanTotalQty).DefaultIfEmpty().Sum();
            RaisePropertyChanged("MPSListView");
            LoadWaitvisibility = Visibility.Collapsed;
        }

        public Visibility LoadWaitvisibility { get => loadWaitvisibility; set { loadWaitvisibility = value; RaisePropertyChanged("LoadWaitvisibility"); } }

        public List<RingItem> CPRingItemView { get; set; }
        public List<BOMItemBase> BOMItemPurchaseView { get; set; }

        private MPSPlanItem selectedMpsProItem = new MPSPlanItem();
        public MPSPlanItem SelectedMPSProItem
        {
            get { return selectedMpsProItem; }
            set
            {
                if (value != null)
                {
                    selectedMpsProItem = value;
                    mpsItemSelected(value);
                    RaisePropertyChanged("SelectedMPSProItem");
                }
            }
        }


        public List<string> WorkShopView
        {
            get => workShopView; set
            {
                workShopView = value;
                RaisePropertyChanged("WorkShopView");
            }
        }
        public string SelectedWorkShop { get => selectedWorkShop; set => WorkShopFilterChanged(value); }

        public List<MPSPlanItem> PlanProItemView { get; set; }
        private List<ICMO> IcmoList { get; set; }
        public ICollectionView MPSListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(PlanProItemView);
            }
        }

        private string mpsFilterText = string.Empty;
        public string MPSFilterText
        {
            get => this.mpsFilterText;
            set
            {
                if (MPSListView != null) ((IEditableCollectionView)MPSListView).CommitEdit();
                FNumberFilterChanged(value);
                RaisePropertyChanged("MPSFIlterText");
            }
        }


        private Dictionary<int, K3CPItemBase> cpItemsDic = new Dictionary<int, K3CPItemBase>();

        private async void mpsItemSelected(MPSPlanItem mpsItem)
        {

            await mpsItem.CPItem.OnItemIDChangedAsync();
            mpsItem.GetProdceYCQty();
            CPRingItemView = mpsItem.CPItem.Rings;
            BOMItemPurchaseView = mpsItem.CPItem.ComponentItems.Where(i => i.ItemTpye == ItemType.Purchase).ToList();

            RaisePropertyChanged("CPRingItemView");
            RaisePropertyChanged("BOMItemPurchaseView");
        }

        private void WorkShopFilterChanged(string value)
        {
            if (value == null) return;
            ((IEditableCollectionView)MPSListView).CommitEdit();   ///https://stackoverflow.com/questions/20204592/wpf-datagrid-refresh-is-not-allowed-during-an-addnew-or-edititem-transaction-m
            value = value.ToUpper();
            if (MPSListView == null) return;
            MPSListView.Filter = new Predicate<object>(x =>
            {
                var vm = (MPSPlanItem)x;
                if (vm.CPItem == null || string.IsNullOrEmpty(vm.CPItem.DepFName)) return false;
                if (string.IsNullOrEmpty(mpsFilterText))
                {
                    return vm.CPItem.DepFName.Contains(value);
                }
                else
                {
                    return vm.CPItem.DepFName.Contains(value) && vm.K3FNumber.Contains(value);
                }
            });

            FilterPlanQty = MPSListView.OfType<MPSPlanItem>().Select(s => s.PlanTotalQty).DefaultIfEmpty().Sum();
            selectedWorkShop = value;
            //MPSListView?.Refresh();
        }
        public int FilterPlanQty { get => filterPlanQty; set { filterPlanQty = value; RaisePropertyChanged("FilterPlanQty"); } }
        private void FNumberFilterChanged(string value)
        {
            if (MPSListView == null) return;
            value = value.ToUpper();
            if (value == "")
            {
                MPSListView.Filter = null;
            }
            MPSListView.Filter = new Predicate<object>(x =>
            {
                var vm = (MPSPlanItem)x;
                return (vm.K3FNumber.Contains(value) || vm.CPItem.K3FModel.Contains(value)) && vm.CPItem.DepFName == selectedWorkShop;

            });

            FilterPlanQty = MPSListView.OfType<MPSPlanItem>().Select(s => s.PlanTotalQty).DefaultIfEmpty().Sum();
            mpsFilterText = value;

        }
        public List<K3Dep> Deps { get; set; }
        public List<K3Item> ProItemsView { get; set; }

        public ObservableCollection<ICInventory> ICInventoryView2 { get; set; }

        public List<QJItem> QJItemView { get; set; }

        private QJItem _selectedQJItem = new QJItem();
        public QJItem SelectedQJItem
        {
            get { return _selectedQJItem; }
            set { _selectedQJItem = value; MPSDataLoad(value); }
        }


        private RelayCommand backWardCommand;

        public RelayCommand BackWardCommand
        {
            get
            {
                if (backWardCommand == null)
                {
                    backWardCommand = new RelayCommand(BackWardCommandExec, CanBackWard);
                }
                return backWardCommand;
            }
        }

        private bool CanBackWard()
        {
            return mpsFilterText.Length > 0;
        }

        private void BackWardCommandExec()
        {
            MPSFilterText = mpsFilterText.Remove(mpsFilterText.Length - 1);
        }

        private RelayCommand filterEraserCommand;

        public RelayCommand FilterEraserCommand
        {
            get
            {
                if (filterEraserCommand == null)
                {
                    filterEraserCommand = new RelayCommand(FilterEraserCommandExec, CanFilterEraser);
                }
                return filterEraserCommand;
            }
        }

        private bool CanFilterEraser()
        {

            return mpsFilterText.Length > 0;
        }


        private void FilterEraserCommandExec()
        {
            MPSFilterText = string.Empty;
        }


        private RelayCommand addTLCommand;

        public RelayCommand AddTLCommand
        {
            get
            {
                if (addTLCommand == null)
                {
                    addTLCommand = new RelayCommand(AddTLCommandExec, CanAddTL);
                }
                return addTLCommand;
            }
        }
        private bool canAddTL = false;
        private bool CanAddTL()
        {
            return selectedMpsProItem != null && canAddTL && selectedMpsProItem.CPItem.CPItemType == CPItemType.自制;
        }

        private void AddTLCommandExec()
        {
            Messenger.Default.Send<MPSPlanItem>(selectedMpsProItem, "SelectedYPToTLView");
        }



        private RelayCommand addYPCommand;

        public RelayCommand AddYPCommand
        {
            get
            {
                if (addYPCommand == null)
                {
                    addYPCommand = new RelayCommand(AddYPCommandExec, CanAddYP);
                }
                return addYPCommand;
            }
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
            var q0 = ICMOContainer.icmoBomcustomerGroup;
            var qq = new ICMOContainer(50021);
            var q1 = ICMOContainer.icmoBomcustomerGroup;
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
        private void AddYPCommandExec()
        {
            ///将IntegratedInfoview的保存至预排表
            ///涉及到新增和修改
            ///
            var _repository = new MPSYPItemRepository();
            var queryall = _repository.GetALL();
            var item = selectedMpsProItem;
            var _item = queryall.Where(w => w.CPItem.K3ItemID == item.K3ItemID).FirstOrDefault();
            if (_item == null)
            {
                MPSYPItem _item1 = new MPSYPItem
                {
                    CPItem = new K3CPItemBase
                    {
                        K3FNumber = item.K3FNumber,
                        K3ItemID = item.K3ItemID,
                        DepFName = item.CPItem.DepFName,
                        DepId = item.CPItem.DepId
                    },
                    Feeder = "wjj",
                    OnePlanQTY = (int)(item.PlanTLQty),
                    QJId = SelectedQJItem.FItemId,
                    IsDeleted = false,
                };
                _repository.Insert(_item1);
            }
            else
            {
                //_item.PlanQty = (int)(item.PlanTLQty);
                _repository.Update(_item);
            }
            SelectedMPSProItem.YPQty += item.PlanTLQty;
            SelectedMPSProItem.PlanTLQty = 0;
        }
        private bool CanAddYP()
        {
            //return selectedMpsProItem != null && selectedMpsProItem.IsValidQty && selectedMpsProItem.CPItem.CPItemType == CPItemType.自制;
            return true;
        }


        //private RelayCommand addICMOCommand;

        //public RelayCommand AddICMOCommand
        //{
        //    get
        //    {
        //        if (addICMOCommand == null)
        //        {
        //            addICMOCommand = new RelayCommand(AddICMOCommandExec, CanAddICMO);
        //        }
        //        return addICMOCommand;
        //    }
        //}

        //private void AddICMOCommandExec()
        //{
        //    MPSPlanItem _planitem = new MPSPlanItem();
        //    _planitem = selectedMpsProItem;
        //    var newicmo = new ICMO
        //    {
        //        FauxQty = _planitem.PlanTLQty,
        //        FBomInterID = _planitem.CPItem.BOMId,
        //        FWorkShop = _planitem.CPItem.DepId,
        //        FPlanCommitDate = DateTime.Now.Date,
        //        FPlanFinishDate = DateTime.Now.Date,
        //        FItemId = _planitem.CPItem.K3ItemID,
        //        QJId = _planitem.QJId, 
        //        WorkId = 55
        //    };
        //    newicmo.InsertICMO();
        //    _planitem.ICMOQty += _planitem.PlanTLQty;
        //    _planitem.PlanTLQty = 0;
        //}
        //private bool CanAddICMO()
        //{
        //    //return selectedMpsProItem != null && selectedMpsProItem.IsValidQty && selectedMpsProItem.CPItem.CPItemType == CPItemType.外购;
        //    return true;
        //}


        private RelayCommand exportExcelCommand;

        public RelayCommand ExportExcelCommand
        {
            get
            {
                if (exportExcelCommand == null)
                {
                    exportExcelCommand = new RelayCommand(ExportExcelCommandExec);
                }
                return exportExcelCommand;
            }
        }

        private void ExportExcelCommandExec()
        {

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
            Clipboard.SetText(selectedMpsProItem.CPItem.K3FNumber);
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
            return selectedMpsProItem != null;
        }

        private void CopyItemFNameCommandExec()
        {
            Clipboard.SetText(selectedMpsProItem.CPItem.K3FModel);
        }



        private RelayCommand copyItemFIDCommand;
        private string selectedWorkShop;
        private List<string> workShopView;

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
            Clipboard.SetText(selectedMpsProItem.K3ItemID.ToString());
        }


        private Visibility loadWaitvisibility = Visibility.Hidden;
        private int filterPlanQty;

        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new RelayCommand(async () => await RefreshCommandExecAsync());
                }
                return refreshCommand;
            }
        }
        private async Task RefreshCommandExecAsync()
        {
            await MPSDataLoad(SelectedQJItem);
        }

        private RelayCommand planQtyGetFocus;
        public RelayCommand PlanQtyGetFocus
        {
            get
            {
                if (planQtyGetFocus == null)
                {
                    planQtyGetFocus = new RelayCommand(PlanQtyGetFocusExec);
                }
                return planQtyGetFocus;
            }
        }
        private void PlanQtyGetFocusExec()
        {
            canAddTL = true;
        }

    }


}