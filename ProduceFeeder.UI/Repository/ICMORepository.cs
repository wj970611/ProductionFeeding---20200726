using ProduceFeeder.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Repository
{
    public class ICMORepository:IDisposable
    {
        private K3BhDBContext _dbcontext = new K3BhDBContext();
        public IQueryable<ICMO> GetAll()
        {
            return _dbcontext.ICMOs.AsNoTracking();

        }
        public IQueryable<ICMO> GetAll(int qjId)
        {
            return _dbcontext.ICMOs.AsNoTracking()
                       .Where(x => x.QJId < qjId && x.FStatus != 3  && x.FauxQty > 0 && x.FItem.FNumber.StartsWith("5.18"));

        }

      public IQueryable<ICMO> GetAllCP()
        {
            return _dbcontext.ICMOs.AsNoTracking()
                       .Where(x =>  x.FauxQty > 0 && x.FItem.FNumber.StartsWith("5.18"));

        }



        /// <summary>
        /// 获取某个区间的，某个型号的计划数量
        /// </summary>
        /// <param name="qjId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IQueryable<ICMO> GetItem(int itemId)
        {
            return GetAll().Where(w => w.FItemId == itemId);
        }


 

        /// <summary>
        /// 获取任务单中还没有完成的任务数量，由于投料的假设是不会对任务单进行全月的投放
        /// 所以，不用考虑期间
        /// </summary>
        /// <param name="qjId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<decimal> GetWorkOnQtyAsync(int itemId)
        {
            var lst = await _dbcontext.ICMOs.Where(x => x.FStatus != 3).Where(x => x.FauxQty - x.FAuxStockQty > 0)
            .Where(x => x.FItemId == itemId).SumAsync(x => x.FauxQty - x.FAuxStockQty);

            return lst;

        }
        public decimal GetWorkOnQty(int itemId)
        {
            return _dbcontext.ICMOs.Where(x => x.FStatus != 3).Where(x => x.FauxQty - x.FAuxStockQty > 0)
                .Where(x => x.FItemId == itemId).Select(s=>s.FauxQty -s.FAuxStockQty).DefaultIfEmpty(0).Sum(); 
        }

        public decimal GetWorkOnLvl1Qty(int itemId)
        {
            var qq = _dbcontext.PPBOMEntrys.Where(w => w.FItemID == itemId && w.ICMO.FStatus != 3);
            return _dbcontext.PPBOMEntrys.Where(w => w.FItemID == itemId && w.ICMO.FStatus != 3)
               .Select(s => s.FQtyMust - s.FAuxQty).DefaultIfEmpty(0).Sum();
        }


        /// <summary>
        /// 原材料计划内，但没有发出（被领用）
        /// </summary>
        public int GetRawNotSendOut { get; set; }



        public int Insert(int fworkshop, int fitemid,int workId,
                    decimal fqty, DateTime plancommitDate, DateTime planfinishDate,
                    int bomInterid, int qj, int unitId,string myBillNo)
        {
            string _sqlText = "declare @intid int;" +
                "declare @fillno varchar(100);" +
    "exec GetICMaxNum 'icmo', @intid output;" +
 "exec p_bm_GetBillNo 85,@fillno out;" +
 "Insert icmo(fbrno, [FInterID], [FBillNo], [FTranType], [FStatus]," +
 "            [FMRP], [FType], [FWorkShop], [FItemID], [FQty]," +
 "                    [FCommitQty], [FPlanCommitDate], [FPlanFinishDate]," +
 "                    [FConveyerID], [FCheckerID], [FBillerID], [FSourceEntryID]," +
 "                    [FClosed], [FNote], [FUnitID], [FAuxCommitQty], [FAuxQty]," +
 "                    [FOrderInterID], [FPPOrderInterID], [FParentInterID]," +
 "                    [FCancellation], [FSupplyID], [FQtyFinish], [FQtyScrap]," +
 "                    [FQtyForItem], [FQtyLost], [FRoutingID], [FAuxQtyFinish]," +
 "                    [FAuxQtyScrap], [FAuxQtyForItem], [FAuxQtyLost], [FMrpClosed]," +
 "                    [FBomInterID], [FQtyPass], [FAuxQtyPass], [FQtyBack]," +
 "                    [FAuxQtyBack], [FFinishTime], [FReadyTIme], [FPowerCutTime]," +
 "                    [FFixTime], [FWaitItemTime], [FWaitToolTime], [FTaskID]," +
 "                    [FWorkTypeID], [FCostObjID], [FStockQty], [FAuxStockQty]," +
 "                    [FSuspend], [FProjectNO], [FProductionLineID], [FReleasedQty]," +
 "                    [FReleasedAuxQty], [FUnScheduledQty], [FUnScheduledAuxQty]," +
 "                    [FSubEntryID], [FScheduleID], [FPlanOrderInterID], [FProcessPrice]," +
 "                    [FProcessFee], [FGMPCollectRate], [FGMPItemBalance], [FGMPBulkQty]," +
 "                    [FCustID], [FMRPLockFlag], [FHandworkClose], [FConfirmerID]," +
 "                    [FInHighLimit], [FInHighLimitQty], [FAuxInHighLimitQty], [FInLowLimit]," +
 "                    [FInLowLimitQty], [FAuxInLowLimitQty], [FChangeTimes], [FCheckCommitQty]," +
 "                    [FAuxCheckCommitQty], [FPlanConfirmed], [FPlanMode], [FMTONo]," +
 "                    [FPrintCount], [FFinClosed], [FFinCloseer], [FStockFlag]," +
 "                    [FStartFlag], [FVchInterID], [FCardClosed], [FHRReadyTime]," +
 "                    [FPlanCategory], [FBomCategory], [FSourceTranType], [FSourceInterId]," +
 "                    [FReprocessedAuxQty], [FReprocessedQty], [FSelDiscardStockInAuxQty]," +
 "                    [FSelDiscardStockInQty], [FDiscardStockInAuxQty], [FDiscardStockInQty]," +
 "                    [FSampleBreakAuxQty], [FSampleBreakQty], [FResourceID], [FAddInterID]," +
 "                    [FAPSImported], [FAPSLastStatus], [FAuxPropID], [FOrderBOMEntryID]," +
 "                    [FIsMakeLowerBill], [FConnectFlag], [FHeadSelfJ01100],[FHeadSelfJ01101])" +
 $"                 values(0, @intid, @fillno, 85, 0," +
 $"                     1052, 1054, {fworkshop}, {fitemid}, {fqty}," +
 $"           0, '{plancommitDate}', '{planfinishDate}'," +
 $"            0, 0, 16394, 0," +
 $"            0, '投料系统添加，数量' + cast({fqty} as nvarchar(255)), {unitId}, 0, {fqty}," +
 $"            0, 0, 0,0, 0, 0, 0,0, 0, 0, 0,0, 0, 0, 0,{bomInterid}, 0, 0, 0," +
 $"            0, 0, 0, 0,0, 0, 0, 0,{workId}, {fitemid}, 0, 0,0, 0, 0, 0," +
 $"            0, 0, 0,0, 0, 0, 0,0, 0, 0, 0,0, 0, 0, 0,5, {fqty * 1.05m}, {fqty * 1.05m}, 0," +
 $"            {fqty},{fqty}, 0, 0,0, 0, 14036, '', 0,0, 0, 14215,0, 0, 1059, 0," +
 $"            1, 36820, 0, 0,0, 0, 0,0, 0, 0,0, 0, 0, 0,0, 0, 0, 0,0, 0, {qj},'{myBillNo}')";

            return _dbcontext.Database.ExecuteSqlCommand(_sqlText);
        }






        public void Dispose()
        {
            _dbcontext?.Dispose();
        }
    }
}
