using ProduceFeeder.UI.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.K3Items
{


    public enum CPItemType
    {
        自制,
        外购
    }


    public class K3CPItemBase : INotifyPropertyChanged
    {
        private int k3ItemID;

        public int ID { get; set; }
        public int K3ItemID
        {
            get { return k3ItemID; }
            set { k3ItemID = value; }
        }

        public string K3FNumber { get; set; }
        public string K3FModel { get; set; }
        public string K3FName { get; set; }

        public int FCustObjId { get; set; }
        public int FRoutingId { get; set; }
        public string DepFName { get; set; }
        public int DepId { get; set; }
        public List<BOMItemBase> ComponentItems { get => componentItems; set => componentItems = value; }
        public int BOMId { get; internal set; }

        private List<BOMItemBase> componentItems = new List<BOMItemBase>();
        private string dhProcess;
        private string lNProcess;
        private string cJGProcess;
        public void OnItemIDChanged()
        {
            GetComponetItems();

            foreach (var item in componentItems)
            {
                if (item.K3FNumber.ContainsAny(new string[] { "/01", "/91" }))
                {
                    item.DXQ = "01";
                }
                if (item.K3FNumber.ContainsAny(new string[] { "/02", "/92" }))
                {
                    item.DXQ = "02";
                }
            }


            List<BOMProcessedItem> _innerProcessItems = new List<BOMProcessedItem>();
            List<BOMProcessedItem> _outerProcessItems = new List<BOMProcessedItem>();

            componentItems.Where(w => w.ItemTpye == ItemType.Processed && w.DXQ == "01")?.ToList().ForEach(x => _outerProcessItems.Add((BOMProcessedItem)x));
            componentItems.Where(w => w.ItemTpye == ItemType.Processed && w.DXQ == "02")?.ToList().ForEach(x => _innerProcessItems.Add((BOMProcessedItem)x));

            InnerRing.RingProcessItems = _innerProcessItems;
            OuterRing.RingProcessItems = _outerProcessItems;

            PrevRCLProcessItem = getPrevRCLItem();
            PrevRCLProcessItems = InnerRing.PrevRCLItems.Concat(OuterRing.PrevRCLItems).ToList();
            NextRCLProcessItems = InnerRing.NextRCLItems.Concat(OuterRing.NextRCLItems).ToList();

            Rings = new List<RingItem> { OuterRing, InnerRing };

            DHProcess = ProcessQty("氮化");
            LNProcess = ProcessQty("冷辗");
            CJGProcess = ProcessQty("车加工");
        }
        public async Task OnItemIDChangedAsync()
        {
            await GetComponetItemsAsync();

            foreach (var item in componentItems)
            {
                if (item.K3FNumber.ContainsAny(new string[] { "/01", "/91" }))
                {
                    item.DXQ = "01";
                }
                if (item.K3FNumber.ContainsAny(new string[] { "/02", "/92" }))
                {
                    item.DXQ = "02";
                }
            }


            List<BOMProcessedItem> _innerProcessItems = new List<BOMProcessedItem>();
            List<BOMProcessedItem> _outerProcessItems = new List<BOMProcessedItem>();

            componentItems.Where(w => w.ItemTpye == ItemType.Processed && w.DXQ == "01")?.ToList().ForEach(x => _outerProcessItems.Add((BOMProcessedItem)x));
            componentItems.Where(w => w.ItemTpye == ItemType.Processed && w.DXQ == "02")?.ToList().ForEach(x => _innerProcessItems.Add((BOMProcessedItem)x));

            InnerRing.RingProcessItems = _innerProcessItems;
            OuterRing.RingProcessItems = _outerProcessItems;

            PrevRCLProcessItem = getPrevRCLItem();
            PrevRCLProcessItems = InnerRing.PrevRCLItems.Concat(OuterRing.PrevRCLItems).ToList();
            NextRCLProcessItems = InnerRing.NextRCLItems.Concat(OuterRing.NextRCLItems).ToList();

            Rings = new List<RingItem> { OuterRing, InnerRing };

            DHProcess = ProcessQty("氮化");
            LNProcess = ProcessQty("冷辗");
            CJGProcess = ProcessQty("车加工");
        }

        private async Task GetComponetItemsAsync()
        {
            if (componentItems.Count == 0)
            {
                var bomde = new BOMDecompose.BOMDecompose(this);
                await bomde.DecomposeAsync();
                componentItems = bomde.BOMItems;
            }
        }
        private void GetComponetItems()
        {
            var bomde = new BOMDecompose.BOMDecompose(this);
            bomde.Decompose();
            componentItems = bomde.BOMItems;
        }




        /// <summary>
        /// 获取热处理前的一道物料
        /// </summary>
        /// <param name="Ritem"></param>
        /// <returns></returns>
        private List<BOMItemBase> getPrevRCLItem()
        {
            if (CPItemType == CPItemType.自制)
            {
                var _innerRclLel = InnerRing.RclLevle;
                var _innerring = componentItems.Where(w => w.DXQ == "02" && w.Level == _innerRclLel + 1).FirstOrDefault();
                var _outerRclLel = OuterRing.RclLevle;
                var _outerring = componentItems.Where(w => w.DXQ == "01" && w.Level == _outerRclLel + 1).FirstOrDefault();
                return new List<BOMItemBase>() { _innerring, _outerring };
            }
            return null;

        }


        #region 产品结构特性

        [NotMapped]
        public string LNProcess { get => lNProcess; set { lNProcess = value; OnPropertyRaised("LNProcess"); } }
        [NotMapped]
        public string CJGProcess { get => cJGProcess; set { cJGProcess = value; OnPropertyRaised("CJGProcess"); } }
        [NotMapped]
        public string DHProcess
        {
            get
            {
                return dhProcess;
            }
            set
            {
                dhProcess = value; OnPropertyRaised("DHProcess");
            }
        }

        private string ProcessQty(string processName)
        {
            var xq = InnerRing?.RingProcessItems.Where(x => x.ProcessName.Contains(processName)).FirstOrDefault() == null ? "" : "02";
            var dq = OuterRing?.RingProcessItems.Where(x => x.ProcessName.Contains(processName)).FirstOrDefault() == null ? "" : "01";
            return xq + " " + dq;
        }

        /// <summary>
        /// 加工件。 
        /// 1、01 每个过程的在制品数量，用来计算最小投放量
        ///  
        /// </summary>
        public List<RingItem> Rings { get; set; }
        /// <summary>
        /// 大小圈热处理的前一道工序
        /// </summary>
        public List<BOMItemBase> PrevRCLProcessItem { get; set; } = new List<BOMItemBase>();
        /// <summary>
        /// 大小圈热处理的前几道工序
        /// </summary>
        public List<BOMProcessedItem> PrevRCLProcessItems { get; set; } = new List<BOMProcessedItem>();

        public List<BOMProcessedItem> NextRCLProcessItems { get; set; } = new List<BOMProcessedItem>();
        /// <summary>
        /// 获取产品是自制还是外购的
        /// 一个产品如果没有磨加工 5.25开头的物料 
        /// 就认为这是个外购产品
        /// 有没有大小圈一个是外购 一个是自制的？
        /// </summary>
        public CPItemType CPItemType
        {
            get
            {
                if (componentItems.Where(w => w.K3FNumber.StartsWith("5.25")).Count() == 2)
                {
                    return CPItemType.自制;
                }
                else
                {

                    return CPItemType.外购;
                }
            }
        }

        /// <summary>
        /// 外圈 包括外圈的加工件
        /// </summary>
        public RingItem OuterRing { get; set; } = new RingItem();
        /// <summary>
        /// 内圈 就一个
        /// </summary> 
        public RingItem InnerRing { get; set; } = new RingItem();



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        #endregion

        #region 库存属性

        public async Task<List<BOMItemBase>> GetComponetItemsConsumAsync(decimal qty)
        {
            List<BOMItemBase> lst = new List<BOMItemBase>();
            if (componentItems.Count == 0) await GetComponetItemsAsync();
            foreach (var item in componentItems)
            {
                item.FAuxQty = qty * item.FBOMQty;
                lst.Add(item);
            }
            return lst;

        }
        /// <summary>
        /// 获取组件的消耗
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public List<BOMItemBase> GetComponetItemsConsum(decimal qty)
        {
            List<BOMItemBase> lst = new List<BOMItemBase>();
            if (componentItems.Count == 0) GetComponetItems();
            foreach (var item in componentItems)
            {
                item.FAuxQty = qty * item.FBOMQty;
                lst.Add(item);
            }
            return lst;

        }

        #endregion
    }
    public struct WeekQty
    {
        public int ZJHQty { get; set; }
        public int ZTLQty { get; set; }
    }
}
