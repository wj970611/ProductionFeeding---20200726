using ProduceFeeder.UI.Models.K3Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    /// <summary>
    /// 轴承的内外圈,圈包含各自的下道工序
    /// </summary>
    ///  

    [NotMapped]
    public class RingItem
    {
        private List<BOMProcessedItem> pairs = new List<BOMProcessedItem>();
        /// <summary>
        /// 套圈的加工工艺过程
        /// </summary>
        public List<BOMProcessedItem> RingProcessItems { get => pairs; set { pairs = value; OnProcessItemsChanged(); } }

        private void OnProcessItemsChanged()
        {
            if (RingProcessItems.Count == 0) return;
            var rcl = RingProcessItems.Where(w => w.ProcessName == "热处理");
            var lvlmax = RingProcessItems.Max(m => m.Level);
            //热处理是第一道工序
            var lvl_rcl = rcl.Select(s => s.Level).FirstOrDefault();
            RclLevle = lvl_rcl;
            if (lvl_rcl == lvlmax)
            {
                //rcl.FirstOrDefault().IsRCLPrev = true;
                RclIsFirst = true;
            }
            else
            {
                RclIsFirst = false;
            }
            NextRCLItems.Clear();
            for (int i = 1; i <= lvl_rcl; i++)
            { 
                NextRCLItems.Add(RingProcessItems[i-1]);
            }
            if (lvlmax > lvl_rcl)
            {
                PrevRCLItems.Clear();
                for (int i = lvl_rcl + 1; i <= lvlmax; i++)
                {
                    PrevRCLItems.Add(RingProcessItems[i - 1]);
                }
            }

        }
         

        #region 加工件在制数

        public string RingProcessName => RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "25").Select(x => x.K3FNumber.Split('.')[2]).FirstOrDefault();
        public int LNInventoryQty =>(int) RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "19").Select(s=>s.InventoryQty).DefaultIfEmpty(0).Sum();
        public int CJGInventoryQty => (int)RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "20").Select(s=>s.InventoryQty).DefaultIfEmpty(0).Sum();
        public int DHInventoryQty => (int)RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "21").Select(s => s.InventoryQty).DefaultIfEmpty(0).Sum();
        public int PMWXInventoryQty => (int)RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "23").Select(s => s.InventoryQty).DefaultIfEmpty(0).Sum();
        public int MJGInventoryQty => (int)RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "25").Select(s => s.InventoryQty).DefaultIfEmpty(0).Sum();
        //public decimal ZPQty => ICInventorys.Select(s => s.FQty).DefaultIfEmpty().Sum();

        #endregion

        public string K3FNumber => RingProcessItems.Where(x => x.K3FNumber.Split('.')[1] == "25").Select(x => x.K3FNumber.Split('.')[2]).FirstOrDefault();

        public int RclLevle { get; set; }
        public bool RclIsFirst { get; private set; }
        public List<BOMProcessedItem> PrevRCLItems { get; set; } = new List<BOMProcessedItem>();
        public List<BOMProcessedItem> NextRCLItems { get; set; } = new List<BOMProcessedItem>();



    }
}
