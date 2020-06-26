//using ProduceFeeder.UI.Models.K3Items;
//using ProduceFeeder.UI.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProduceFeeder.UI.Models.BOMItemQuery
//{
//    /// <summary>
//    /// 成品物料的 配件匹配查询基类
//    /// </summary>
//    public abstract class CPMaterialQueryBase
//    {
//        private int cpItemId;
//        public CPMaterialQueryBase(int itemId)
//        {
//            cpItemId = itemId;
//        }
//        private MPSICMO proItem;
//        public CPMaterialQueryBase(MPSICMO proitem)
//        {
//            proItem = proitem;
//            cpItemId = proitem.K3ItemID;
//        }

//        //先用金蝶中获取计划状态下的成品，和预排和投料表中合并
//        public async Task<K3CPItemBase> GetUseableItemsAsync()
//        { 

//            JH_ICMO = new ICMORepository().GetAllCP().Where(w => w.FCancellation==false &&  w.FStatus == 0 && w.FItemId == cpItemId)
//                .Select(s => new ItemPOCO { ItemID = cpItemId,   Qty = (int)s.FauxQty }).ToList();
//            //获取预排数量
//            YPList = new Repository.MPSYPItemRepository().GetALL().Where(w => w.CPItem.K3ItemID == cpItemId)
//                .Select(s => new ItemPOCO { ItemID = cpItemId,   Qty = s.Qty }).ToList(); 

//            var concatNoused = JH_ICMO.Concat(YPList).GroupBy(g =>  g.ItemID )
//                .Select(s => new ItemPOCO { ItemID = s.Key ,  Qty = s.Sum(m => m.Qty) })
//                .FirstOrDefault();

//            var newCpItembase =new K3CPItemBase(); 

//            //if ( newCpItembase.ComponentItems==null || newCpItembase.ComponentItems.Count==0) await newCpItembase.InIAsync(); 

//            /////2.对某成品，进行分解，得到零部件的需数,也就是已经分配的数量所占用的资源分解数
//            /////3.对零部件获取库存
//            //newCpItembase.ComponentItems.ForEach(f =>
//            //{
//            //    //物料消耗量
//            //    //f.FAuxQty = newCpItembase.Qty * f.FBOMQty;
                 
//            //    f.GetInventory();
//            //    f.YPQty = YPList.Sum(s => s.Qty) * f.FBOMQty;
//            //    f.JHICMOQty = JH_ICMO.Sum(s => s.Qty) * f.FBOMQty; 
//            //});

//            return   newCpItembase;
//        }


//        public List<ItemPOCO> JH_ICMO { get; private set; }
//        public List<ItemPOCO> YPList { get; private set; }

//    }
    
//}
