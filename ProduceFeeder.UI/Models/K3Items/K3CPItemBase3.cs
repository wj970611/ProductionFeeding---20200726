//using ProduceFeeder.UI.Extension;
//using ProduceFeeder.UI.Interface;
//using ProduceFeeder.UI.Models.K3Items;
//using ProduceFeeder.UI.Repository;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProduceFeeder.UI.Models
//{


//    /// <summary>
//    /// 金蝶的产品类，以5.18带头的物料
//    /// 预排的作用是对选择需要投料的计划，进行齐套计算，工序节奏等待
//    /// </summary>
//    /// 
//    public class K3CPItemBase
//    {

    
//        /// <summary>
//        /// 最小可用数量,
//        /// </summary>
//        /// 

//        private int maxvacantYPQty;
//        private int maxvacantTLQty;
//        private int maxvacantICMOQty;
//        public int MaxVacantYPQty
//        {
//            get
//            {
//                if (componentItems.Count > 0)
//                {
//                    maxvacantYPQty = (int)componentItems.Min(m => m.MaxItemVacantYPQty);
//                    return maxvacantYPQty;
//                }
//                else
//                {
//                    return 0;
//                }
//            }
//            set
//            {
//                maxvacantYPQty = value;
//                OnPropertyRaised("MaxVacantYPQty");
//            }
//        }

//        public int MaxVacantTLQty
//        {
//            get
//            {
//                if (componentItems.Count > 0)
//                {
//                    maxvacantTLQty = (int)componentItems.Min(m => m.MaxItemVacantTLQty);
//                    return maxvacantTLQty;
//                }
//                else
//                {
//                    return 0;
//                }
//            }
//            set
//            {
//                maxvacantTLQty = value;
//                OnPropertyRaised("MaxVacantTLQty");
//            }
//        }

//        public int MaxVacantICMOQty
//        {
//            get
//            {
//                if (componentItems.Count > 0)
//                {
//                    maxvacantICMOQty = (int)componentItems.Min(m => m.MaxItemVacantICMOQty);
//                    return maxvacantICMOQty;
//                }
//                else
//                {
//                    return 0;
//                }
//            }
//            set
//            {
//                maxvacantICMOQty = value;
//                OnPropertyRaised("MaxVacantICMOQty");
//            }
//        }
      
//        #region 产品结构



//        /// <summary>
//        /// 第一道的投料 物料。精车件
//        /// </summary>
//        public List<BOMItemBase> RawItems => componentItems.Where(w => w.GetType() == typeof(BOMRawItem)).ToList();

         
 



//        ///成品的零部件
//        ///
//        /// 
//        /// 
//        #region Methods
           

//        private List<BOMItemBase> componetItemsConsum = new List<BOMItemBase>();
      

       

//        /// <summary>
//        /// 对任务的下单，可以分任务单前的工序下达
//        /// 任务单工序后的下达。以及外购的下达
//        /// </summary>
//        #endregion

//        #endregion


//        #region 库存信息
//        ///查找某个型号在预排 投料 计划中的数量，
//        ///分解改型号 获得子项
//        ///循环历遍每个子项 对照库存 得出可用数。
//        ///这里存在一个其他型号公用子项的情况
//        //public async Task<K3CPItemBase> GetUseableItemsAsync()
//        //{

//        //    JH_ICMO = new ICMORepository().GetAllCP().Where(w => w.FCancellation == false && w.FStatus == 0 && w.FItemId == cpItemId)
//        //        .Select(s => new CPItemPOCO { ItemID = cpItemId, ItemFNumber = s.FItem.FNumber, Qty = (int)s.FauxQty }).ToList();
//        //    //获取预排数量
//        //    YPList = new Repository.MPSYPItemRepository().GetALL().Where(w => w.K3ItemID == cpItemId)
//        //        .Select(s => new CPItemPOCO { ItemID = cpItemId, ItemFNumber = s.K3FNumber, Qty = s.Qty }).ToList();

//        //    var concatNoused = JH_ICMO.Concat(YPList).GroupBy(g => new { g.ItemID, g.ItemFNumber })
//        //        .Select(s => new CPItemPOCO { ItemID = s.Key.ItemID, ItemFNumber = s.Key.ItemFNumber, Qty = s.Sum(m => m.Qty) })
//        //        .FirstOrDefault();

//        //    var newCpItembase = proItem ?? new MPSICMO { K3ItemID = cpItemId, Qty = concatNoused == null ? 0 : concatNoused.Qty };

//        //    if (newCpItembase.ComponentItems == null || newCpItembase.ComponentItems.Count == 0) await newCpItembase.InIAsync();

//        //    ///2.对某成品，进行分解，得到零部件的需数,也就是已经分配的数量所占用的资源分解数
//        //    ///3.对零部件获取库存
//        //    newCpItembase.ComponentItems.ForEach(f =>
//        //    {
//        //        //物料消耗量
//        //        f.FAuxQty = newCpItembase.Qty * f.FBOMQty;

//        //        f.GetInventory();
//        //        f.YPQty = YPList.Sum(s => s.Qty) * f.FBOMQty;
//        //        f.JHICMOQty = JH_ICMO.Sum(s => s.Qty) * f.FBOMQty;
//        //    });

//        //    return newCpItembase;
//        //}

//        public async Task testAsync()
//        {
//            var cpitemid = 28975;
//            var bomitems = await BOMItemBaseFactory.CreateK3CPItemCreateAsync(cpitemid);

//        }
//        #endregion

//        #region 计划

//        #endregion




//        /// <summary>
//        /// 成品数量的反算数量
//        /// </summary>
//        /// <returns></returns>
//        //public int CalCPQty()
//        //{
//        //    //在预排前的成品反算公式：预排+待投+生产已投=可期的出品量

//        //}
//    }

//}
