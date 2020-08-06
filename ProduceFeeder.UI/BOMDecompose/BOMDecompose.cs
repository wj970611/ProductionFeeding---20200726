using DevExpress.Xpf.Ribbon;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.K3Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.BOMDecompose
{
    /// <summary>
    /// 该类是否是改造成静态类比较好
    /// </summary>
    public class BOMDecompose
    {
        public static readonly Dictionary<int, List<BOMItemBase>> DicBOMItems = new Dictionary<int, List<BOMItemBase>>();
        public BOMDecompose(DbContext dbContext,K3CPItemBase cpitem)
        {
            _dbContext = dbContext;
            _cpItem = cpitem;
        }
        public BOMDecompose(K3CPItemBase cpitem)
        {
            _cpItem = cpitem;
            _dbContext = new K3BhDBContext();
        }
        private K3CPItemBase _cpItem = new K3CPItemBase();
        private DbContext _dbContext;
        private string _cpFnumber = string.Empty;
        private int _level = 0;
         
        public List<BOMItemBase> BOMItems { get; set; } = new List<BOMItemBase>();
        public void Decompose()
        {
            BOMItems.Clear();

            if (DicBOMItems.ContainsKey( _cpItem.K3ItemID))
            {
                BOMItems = DicBOMItems[_cpItem.K3ItemID];
                return;
            }

            //查询入库 成品
            var _cpBOMItem = _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == _cpItem.K3ItemID && x.FStatus == "审核" && x.FUseStatus == "使用").ToList();

            if (_cpBOMItem != null && _cpBOMItem.Count > 0)
            {
                _cpItem.BOMId = _cpBOMItem[0].FInterID;
                _cpFnumber = _cpBOMItem[0].FNumber;
                foreach (var item in _cpBOMItem)
                {
                    _level = 0;

                    var _cpBOMItemChild = _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == item.ChildItemID && x.FStatus == "审核" && x.FUseStatus == "使用").FirstOrDefault();
                   ///如果成品没有下级子项
                    if (_cpBOMItemChild == null)
                    {
                        BOMItems.Add(new BOMComponentItem
                        {
                            K3FNumber = item.ProChildItem.FNumber,
                            K3FModel = item.ProChildItem.FModel,
                            K3FName = item.FChildName,
                            CPFNumber = _cpFnumber,
                            FBOMQty = item.FChildQty,
                            K3ItemID = item.ChildItemID,
                            FScrap = item.FScrap,
                            IsLastOne = true,
                            ItemTpye = ItemType.Purchase, 
                            Level = 1,
                        });
                    }
                    else
                    { 
                        ReRecursionDecompose(item);
                    }
                } 
            }
            DicBOMItems[_cpItem.K3ItemID] = BOMItems;
        }
        /// <summary>
        /// 进入二级循环
        /// </summary>
        /// <param name="item"></param>
        private void ReRecursionDecompose(ICBOM item)
        {
            _level++;
            var _bomitem = _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == item.ChildItemID && x.FStatus == "审核" && x.FUseStatus == "使用").FirstOrDefault();
           
            if (_bomitem == null)
            {

                BOMItems.Add(new BOMRawItem
                {
                    K3ItemID = item.ProChildItem.ID,
                    FBOMQty = item.FChildQty,
                    K3FNumber = item.ProChildItem.FNumber,
                    K3FName = item.FChildName,

                    K3FModel = item.ProChildItem.FModel,
                    FScrap = item.FScrap,
                    IsLastOne = true,
                    CPFNumber = _cpFnumber,
                    ItemTpye = ItemType.Purchase,
                    Level = _level,
                    FCustObjId = Repository.CustObjReposity.GetItemIdByFNumber(item.ProChildItem.FNumber)

                }) ;
                //BOMItems.Add(item);
                return;
            }
            BOMItems.Add(new BOMProcessedItem
            {
               
                K3ItemID = item.ProChildItem.ID,
                FBOMQty = item.FChildQty,
                K3FNumber = item.ProChildItem.FNumber,
                K3FName = item.FChildName,
                K3FModel = item.ProChildItem.FModel,
                FScrap = item.FScrap,
                IsLastOne = false,
                CPFNumber = _cpFnumber,
                ItemTpye = ItemType.Processed,
                ProcessName = item.FChildName,
                DepId = _bomitem.DepID,
                Level = _level,
                BOMId = _bomitem.FInterID,
                FCustObjId = Repository.CustObjReposity.GetItemIdByFNumber(item.ProChildItem.FNumber)
            });
            ReRecursionDecompose(_bomitem);
        }
        public async Task DecomposeAsync()
        {
            BOMItems.Clear();

            var _cpBOMItem = await _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == _cpItem.K3ItemID && x.FStatus == "审核" && x.FUseStatus == "使用").ToListAsync();

            if (_cpBOMItem != null && _cpBOMItem.Count > 0)
            {
                _cpItem.BOMId = _cpBOMItem[0].FInterID; 
                _cpFnumber = _cpBOMItem[0].FNumber;
                foreach (var item in _cpBOMItem)
                {
                    _level = 0;
                    var _cpBOMItemChild = _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == item.ChildItemID && x.FStatus == "审核" && x.FUseStatus == "使用").FirstOrDefault();
                    if (_cpBOMItemChild == null)
                    {
                        BOMItems.Add(new BOMComponentItem
                        {
                            K3FNumber = item.ProChildItem.FNumber,
                            K3FModel = item.ProChildItem.FModel,
                            K3FName = item.FChildName,
                            CPFNumber = _cpFnumber,
                            FBOMQty = item.FChildQty,
                            K3ItemID = item.ChildItemID,
                            FScrap = item.FScrap,
                            IsLastOne = true,
                            ItemTpye = ItemType.Purchase,
                            DepId = item.DepID,
                            Level = 1,
                            FCustObjId = Repository.CustObjReposity.GetItemIdByFNumber(item.ProChildItem.FNumber)
                        });
                    }
                    else
                    {
                        item.FBOMNumber = _cpBOMItemChild.FBOMNumber;
                        item.FInterID = _cpBOMItemChild.FInterID;
                        await ReRecursionDecomposeAsync(item);
                    }
                }
            }
        }

        private async Task ReRecursionDecomposeAsync(ICBOM item)
        {
            _level++;
            var _bomitem = await _dbContext.Set<ICBOM>().Include("ProChildItem").Where(x => x.FatherItemId == item.ChildItemID && x.FStatus == "审核" && x.FUseStatus == "使用").FirstOrDefaultAsync();
            if (_bomitem == null)
            {
                BOMItems.Add(new BOMRawItem
                {
                    K3ItemID = item.ProChildItem.ID,
                    FBOMQty = item.FChildQty,
                    K3FNumber = item.ProChildItem.FNumber,
                    K3FName = item.FChildName,
                    K3FModel = item.ProChildItem.FModel,
                    FScrap = item.FScrap,
                    IsLastOne = true,
                    CPFNumber = _cpFnumber,
                    ItemTpye = ItemType.Purchase,
                    DepId = item.DepID,
                    Level = _level,
                    FCustObjId = Repository.CustObjReposity.GetItemIdByFNumber(item.ProChildItem.FNumber)
                });
            }
            BOMItems.Add(new BOMProcessedItem
            {
                BOMId=item.FInterID,
                K3ItemID = item.ProChildItem.ID,
                FBOMQty = item.FChildQty,
                K3FNumber = item.ProChildItem.FNumber,
                K3FName = item.FChildName,
                K3FModel = item.ProChildItem.FModel,
                FScrap = item.FScrap,
                IsLastOne = false,
                CPFNumber = _cpFnumber,
                ItemTpye = ItemType.Processed,
                ProcessName = item.FChildName,
                DepId = item.DepID,
                Level = _level,
                FCustObjId = Repository.CustObjReposity.GetItemIdByFNumber(item.ProChildItem.FNumber)
            });
            ReRecursionDecompose(_bomitem);
        }
    }
}
