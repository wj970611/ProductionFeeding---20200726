using ProduceFeeder.UI.Models.BOMItemQuery;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.ItemsContainer
{
    public struct ItemPOCO
    {
        public int ItemID { get; set; }
        public decimal Qty { get; set; }
    }
    /// <summary>
    /// 预排的集合
    /// </summary>
    public class MPSYPContainer : IMaterialWatcher
    {

        public static List<ItemPOCO> ypBomcustomerGroup = new List<ItemPOCO>();


        static MPSYPContainer()
        {

            OnItemsChanged();
        }
        static List<MPSYPItem> GetAllCP()
        {
            return new Repository.MPSYPItemRepository().GetALL().ToList();
        }
     
 
        static void CPDeBOMDecompose()
        {
            //获取预排表的所有物料
            var cpitems = GetAllCP();
            List<BOMItemBase> bomitems = new List<BOMItemBase>(); 
            foreach (var item in cpitems)
            {
                bomitems = bomitems.Concat(item.CPItem?.GetComponetItemsConsum(item.OnePlanQTY)).ToList();
            }
            ypBomcustomerGroup = bomitems.GroupBy(g => g.K3ItemID)
                .Select(s => new ItemPOCO { ItemID = s.Key, Qty = s.Sum(m => m.FAuxQty) }).ToList();
        }

      


        static void Insert(MPSYPItem item)
        {
            OnItemsChanged();
        }
        static void Update(MPSYPItem item)
        {


            OnItemsChanged();
        }
        static void Delete(MPSYPItem item)
        {

            OnItemsChanged();
        }

        static void OnItemsChanged()
        {
            GetAllCP();
            CPDeBOMDecompose(); 
        }

    }
}
