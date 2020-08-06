 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using ProduceFeeder.UI.Models.YuPai;

namespace ProduceFeeder.UI.Models.ItemsContainer
{
    /// <summary>
    /// 投料的集合
    /// </summary>
    public class MPSTLContainer : IMaterialWatcher, IBearingMatch
    {
        public static List<ItemPOCO> tlBomcustomerGroup = new List<ItemPOCO>();

        static MPSTLContainer()
        { 
            OnItemsChanged();
        }
        static IQueryable<MPSTLItem> GetAllCP()
        {
            return new Repository.TLScheduleRepository().GetALL().Where(w => w.RunningStatus != TLRunningStatus.完成);
        }  

        public static decimal  WaittingTLQtyBYCPFNumber(string itemFnumber)
        {
            return  GetAllCP().Where(w=> w.CPItem.K3FNumber == itemFnumber).Select(s => s.Qty).DefaultIfEmpty().Sum();
        }

        public  static async Task<int> WaittingTLQtyBYCPFNumberAsync(string itemFnumber)
        {
            return await GetAllCP().Where(w =>  w.CPItem.K3FNumber == itemFnumber)
                .Select(s => s.Qty)
                .DefaultIfEmpty()
                .SumAsync();
        }



        static List<BOMItemBase> bomitems = new List<BOMItemBase>();  


        /// <summary>                                          
        /// BOM分解 ，为什么要BOM分解？                                          
        /// </summary>
        static void CPDeBOMDecompose()
        {
            //获取所有投料单成品
            var cpitems = GetAllCP();
            //循环
            foreach (var item in cpitems)
            {
                //每个成品都进行分解
                var _items = item.CPItem?.GetComponetItemsConsum(item.Qty);
                if (_items != null)
                {
                    bomitems = bomitems.Concat(_items).ToList();
                }
            }
        } 

        static void BOMItemGroup()
        {
            tlBomcustomerGroup = bomitems.GroupBy(g => g.K3ItemID)
                .Select(s => new ItemPOCO { ItemID = s.Key, Qty = s.Sum(m => m.FAuxQty) }).ToList();
        }


        static void Insert(MPSTLItem item)
        {
            OnItemsChanged();
        }
        /// <summary>
        /// 异步方法有可能造成2条记录的录入
        /// </summary>
        /// <param name="item"></param>
       public static  void Update(MPSTLItem item)
        {
            //OnItemsChanged();

            var _repository = new Repository.TLScheduleRepository();
            ///获取待更新的计划stamp
            var _planstamp = item.PlanStamp; 
            if (_repository.GetALL().Where(w => w.PlanStamp == _planstamp && w.OutFeedingDate != null).Count() ==1)
            {
                //说明已经完成计划投料了 ，可以下任务了。 
                var cpitem = item.CPItem;
                //成品分解
               cpitem.OnItemIDChanged();
                //成品分解后的子项的赋值
                cpitem.GetComponetItemsConsum(item.Qty); 
                cpitem.NextRCLProcessItems.ForEach(x =>
                {
                    new ICMO
                    {
                        FauxQty = x.FAuxQty,
                        FBomInterID = x.BOMId,
                        FWorkShop = x.DepId,
                        FPlanCommitDate = DateTime.Now.Date,
                        FPlanFinishDate = DateTime.Now.Date,
                        FItemId = x.K3ItemID,
                        QJId = item.QJId,
                        WorkId =x.WorkID,
                        FCostObjId=x.FCustObjId,
                        FRoutingId=x.FRoutingId,
                        UnitID=305,
                        TLBillNo=item.PlanStamp,
                        CPItemId=cpitem.K3ItemID
                    }.InsertICMO();
                });
                //成品
                new ICMO
                {
                    FauxQty = item.Qty,
                    FBomInterID = cpitem.BOMId,
                    FWorkShop = cpitem.DepId,
                    FPlanCommitDate = DateTime.Now.Date,
                    FPlanFinishDate = DateTime.Now.Date,
                    FItemId = cpitem.K3ItemID,
                    QJId = item.QJId,
                    WorkId = 55,
                    FCostObjId = item.CPItem.FCustObjId,
                    FRoutingId = item.CPItem.FRoutingId,
                    UnitID = 293,
                    TLBillNo = item.PlanStamp,
                    CPItemId = cpitem.K3ItemID
                }.InsertICMO();
            } 
                item.OutFeedingDate=DateTime.Now;
                item.RunningStatus = TLRunningStatus.完成;
                _repository.Update(item); 
          

        }
        static void Delete(MPSTLItem item)
        { 
            OnItemsChanged();
        }

        static void OnItemsChanged()
        {
            CPDeBOMDecompose();
            BOMItemGroup();
        }
    }
}

