using ProduceFeeder.UI.Models.ItemsContainer;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    public enum ItemType
    {
        Processed,
        Purchase,
        Raw
    }


    /// <summary>
    /// 定义一个bom分解后的物料类。
    /// </summary>
    [NotMapped]
    public abstract class BOMItemBase : INotifyPropertyChanged
    {
     
        private decimal validTLQty;

        //internal K3BhDBContext _k3context = new K3BhDBContext();

        private string k3FNumber;
        public int ID { get; set; }

        public string K3FNumber
        {
            get => k3FNumber; set
            {if (value != null && value.StartsWith("5.25"))
                {
                    WorkID = 69;
                  
                    if (DXQ=="01")
                    {
                        FRoutingId = 1001;
                    }
                    if (DXQ=="02")
                    {
                        FRoutingId = 1002;
                    }
                }
                else
                {
                    WorkID = 55;
                }
                k3FNumber = value; 
            }
        }
         
        public int  FCustObjId { get; set; }
        public int FRoutingId { get; set; }

        public int WorkID { get; internal set; }
        public string K3FModel { get; set; }
        //外购还是自制
        //public string K3ClsName { get; set; }
        public string K3FName { get; set; }
        public string K3FUnitName { get; set; }
        public int K3ItemID { get; set; }
        public string CPFNumber { get; set; }
        public int DepId { get; set; }
        /// <summary>
        /// 折损率
        /// </summary>
        public decimal FScrap { get; set; }
        /// <summary>
        /// bom 配数
        /// </summary>
        public decimal FBOMQty { get; set; }
        public decimal FAuxQty { get; set; }
        public bool IsLastOne { get; set; }

        public int BOMId { get; internal set; }
        public string DXQ { get; set; }
        public ItemType ItemTpye { get; set; }

        /// <summary>
        /// 设置是否可以热处理的工序
        /// </summary>
        public bool CanRCL { get; internal set; } = false;
        /// <summary>
        /// BOM level
        /// </summary>
        public int Level { get; set; }


        #region 库存数 

        /// <summary>                                        
        /// 实库情况                                
        /// </summary>
        public List<ICInventory> ICInventorys { get; set; }
        /// <summary>                                        
        /// 虚库情况                                
        /// </summary>
        public List<POInventory> POInventorys { get; set; }
        public decimal ICInventoryQty => OnICInventory().Select(s => s.FQty)
            .DefaultIfEmpty(0)
            .Sum();
        public decimal POInventoryQty => OnPOInventory().Select(s => s.FQty).DefaultIfEmpty(0).Sum();

        public decimal InventoryQty => ICInventoryQty + POInventoryQty + IncomingNoCheckedQty;
        /// <summary>
        /// 来料未检的数量，只有外购的原配件还有这个数量
        /// </summary>
        public decimal IncomingNoCheckedQty { get; private set; }

        #endregion
        #region 消耗数

        /// <summary>
        /// 物料在待投的总消耗数量
        /// </summary>
        public decimal ItemTLTotalQty { get; set; }
        /// <summary>
        /// 物料在预排的总消耗数量
        /// </summary>
        public decimal ItemYPTotlQty { get; internal set; }
        /// <summary>
        /// 有计划 未领数量 对于配件则需要在投料单中去查找
        /// </summary>
        public decimal ItemNoSendOutQty { get; set; }

        public decimal ItemICMO_JHQty { get; internal set; }
        #endregion

        /// <summary>
        /// 这是库存-预排-投料-计划的可用数量
        /// </summary>
        public decimal ItemUseableQty => ICInventoryQty + POInventoryQty + IncomingNoCheckedQty - ItemYPTotlQty - ItemTLTotalQty;




        /// <summary>
        /// 这是remainQty +预排的数量，实际可用来投料的数量
        /// </summary>
        ///  
        public decimal ValidTLQty
        {
            get => validTLQty;
            set
            {
                if (value != 0)
                {
                    ItemOutputQty = value / FBOMQty;
                }
                validTLQty = value;
            }
        }
        /// <summary>
        /// 这是配件的组装数 
        /// 
        /// </summary>
        private decimal itemOutputQty;

        public decimal ItemOutputQty
        {
            get { return itemOutputQty; }
            set { itemOutputQty = value; OnPropertyRaised("ItemOutputQty"); }
        }


        public List<ICInventory> OnICInventory()
        {
            if (ICInventorys == null)
            {
                GetInventory();
            }
            return ICInventorys;
        }
        public List<POInventory> OnPOInventory()
        {
            if (POInventorys == null)
            {
                GetInventory();
            }
            return POInventorys;
        }
        public async Task<List<ICInventory>> OnICInventoryAsnycAsync()
        {
            if (ICInventorys == null)
            {
                using (K3BhDBContext _context = new K3BhDBContext())
                {
                    ICInventorys = await ICInventory.GetICInventoriesAsync(K3ItemID);
                }
            }
            return ICInventorys;
        }
        public async Task<List<POInventory>> OnPOInventoryAsnycAsync()
        {
            if (POInventorys == null)
            { 
                using (K3BhDBContext _context = new K3BhDBContext())
                {
                    POInventorys = await POInventory.GetPOInventoriesAsync(K3ItemID);
                }
            }
            return POInventorys;
        }
        public async Task GetInventoryAsync()
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            {
                ICInventorys = await ICInventory.GetICInventoriesAsync(K3ItemID);
                POInventorys = await POInventory.GetPOInventoriesAsync(K3ItemID);
                IncomingNoCheckedQty = ItemTpye == ItemType.Processed ? 0 : await K3SongJianDan.GetNoCheckItemQtyAsync(K3ItemID);
                //ItemTLQty =   await      new Repository.TLScheduleRepository().WaittingTLQtyAsync( K3ItemID);
                ItemYPTotlQty = MPSYPContainer.ypBomcustomerGroup.Where(w => w.ItemID == K3ItemID).Select(s => s.Qty).DefaultIfEmpty(0).Sum();
                ItemTLTotalQty = MPSTLContainer.tlBomcustomerGroup.Where(w => w.ItemID == K3ItemID).Select(s => s.Qty).DefaultIfEmpty(0).Sum();
                var newicmocontainer = new ICMOContainer(K3ItemID);
                ItemICMO_JHQty = ICMOContainer.icmoBomcustomerGroup.Select(s => s.Qty).DefaultIfEmpty(0).Sum();
                ItemNoSendOutQty = ItemTpye != ItemType.Purchase ? 0 : await new Repository.ICMORepository().GetWorkOnQtyAsync(K3ItemID);

                //这里能得到实库 虚库 投料 未领 的数量  

            }
        }
        public void GetInventory()
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            {
                ICInventorys = ICInventory.GetICInventories(K3ItemID);
                POInventorys = POInventory.GetPOInventories(K3ItemID);
                IncomingNoCheckedQty = ItemTpye == ItemType.Processed ? 0 : K3SongJianDan.GetNoCheckItemQty(K3ItemID);
                ItemYPTotlQty = MPSYPContainer.ypBomcustomerGroup.Where(w => w.ItemID == K3ItemID).Select(s => s.Qty).DefaultIfEmpty(0).Sum();
                ItemTLTotalQty = MPSTLContainer.tlBomcustomerGroup.Where(w => w.ItemID == K3ItemID).Select(s => s.Qty).DefaultIfEmpty(0).Sum();
                ItemNoSendOutQty = ItemTpye != ItemType.Purchase ? 0 : new Repository.ICMORepository().GetWorkOnLvl1Qty(K3ItemID);

                //这里能得到实库 虚库 投料 未领 的数量  
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }




    }
}
