using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.ItemsContainer;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProduceFeeder.UI.ViewModel
{
    public class ERKeViewModel : ViewModelBase
    {

        public ERKeViewModel()
        {
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

                PlanProItemView = await new K3MPSRepository().GetAll().Where(x => x.K3Dep.FName == "制造二科")
                    .Where(x => x.QJId == qJItem.FItemId)
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
                return (vm.K3FNumber.Contains(value) || vm.CPItem.K3FModel.Contains(value));

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

        public string StatusTitle { get => statusTitle; set { statusTitle = value; RaisePropertyChanged("StatusTitle"); } }
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


        private RelayCommand addICMOCommand;

        public RelayCommand AddICMOCommand
        {
            get
            {
                if (addICMOCommand == null)
                {
                    addICMOCommand = new RelayCommand(AddICMOCommandExec, CanAddICMO);
                }
                return addICMOCommand;
            }
        }

        private void AddICMOCommandExec()
        {
            MPSPlanItem _planitem = new MPSPlanItem();
            _planitem = selectedMpsProItem;
            var newicmo = new ICMO
            {
                FauxQty = _planitem.PlanTLQty,
                FBomInterID = _planitem.CPItem.BOMId,
                FWorkShop = _planitem.CPItem.DepId,
                FPlanCommitDate = DateTime.Now.Date,
                FPlanFinishDate = DateTime.Now.Date,
                FItemId = _planitem.CPItem.K3ItemID,
                QJId = _planitem.QJId,
                WorkId = 55,UnitID=293
            };
            newicmo.InsertICMO();
            _planitem.ICMOQty += _planitem.PlanTLQty;
            _planitem.PlanTLQty = 0;
            PlanTLQty ="";
            StatusTitle = $"任务添加成功！新增任务单号{ ICMO.MaxFBillNo()}";
        }
        private bool CanAddICMO()
        {
            return selectedMpsProItem != null && selectedMpsProItem.IsValidQty && selectedMpsProItem.CPItem.CPItemType == CPItemType.外购;
            //return true;
        }


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


        private RelayCommand refreshCommand;
        private Visibility loadWaitvisibility = Visibility.Hidden;
        private int filterPlanQty;
        private string statusTitle;

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
            //canAddTL = true;
        }




        private RelayCommand searchTextboxEnterKeyCommand;
        private string planTLQty;

        public RelayCommand SearchTextboxEnterKeyCommand
        {
            get
            {
                if (searchTextboxEnterKeyCommand == null)
                {
                    searchTextboxEnterKeyCommand = new RelayCommand(SearchTextboxEnterKeyCommandExec);
                }
                return searchTextboxEnterKeyCommand;
            }
        }

        public string PlanTLQty { get => planTLQty;   set { planTLQty = value;RaisePropertyChanged("PlanTLQty"); } }

        private void SearchTextboxEnterKeyCommandExec()
        {
            /// <summary>
            /// 由DataTable计算公式
            /// </summary>
            /// <param name="expression">表达式</param>
            try
            {
                var qty = PlanTLQty;
                if (PlanTLQty.StartsWith("="))
                {
                    qty = qty.Replace("=", "");
                }
                object result = new DataTable().Compute(qty, "");
                PlanTLQty = result.ToString();
                selectedMpsProItem.PlanTLQty = int.Parse(PlanTLQty);
            }
            catch (Exception ex)
            {
                StatusTitle = ex.Message;
            }

        }

    }
}

