using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 金蝶的生产任务单
    /// </summary>
    /// 
    [Table("ICMO")]
    public class ICMO
    {
        [Key]
        [Column("FInterID")]
        public int ID { get; set; }
        public int FItemId { get; set; }

        public virtual K3Item FItem { get; set; }


        public string FBillNo { get; set; }
        [Column("FHeadSelfJ01100")]
        public int? QJId { get; set; }

        [Column("FHeadSelfJ01101")]
        public string myBillNo { get; set; }
        [NotMapped]
        public decimal VaildQty { get => FauxQty - FAuxStockQty; }
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal FauxQty { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal FAuxStockQty { get; set; }

        /// <summary>
        /// 是否作废 1作废 0 正常
        /// </summary>
        public bool FCancellation { get; set; }

        /// <summary>
        /// 1下达  5是确认 3是结案 0是计划
        /// </summary>
        [Column(TypeName = "smallint")]
        public short FStatus { get; set; } = 0;




        public string FSourceBillNo { get; set; }
        public int FSourceInterId { get; set; }

        /// <summary>
        /// 单据下达日期
        /// </summary>
        public DateTime? FCommitDate { get; set; }

        /// <summary>
        /// 计划开工日期
        /// </summary>
        public DateTime? FPlanCommitDate { get; set; }
        public DateTime? FPlanFinishDate { get; set; }

        public int FWorkShop { get; set; }

        public int FBomInterID { get; set; }
        public int WorkId { get; set; }
        public int UnitID { get; set; }
        public  int InsertICMO()
        {   
            return new Repository.ICMORepository().Insert(FWorkShop,FItemId,WorkId,FauxQty,FPlanCommitDate.GetValueOrDefault().Date,
                            FPlanFinishDate.GetValueOrDefault().Date,FBomInterID,QJId.GetValueOrDefault(), UnitID, myBillNo);
        }

        internal static string MaxFBillNo()
        { 
            return new Repository.ICMORepository().GetAll().Max(x=>x.FBillNo);
        }

        //public ICollection<PPBOMEntry> PPBOMEntry { get; set; }




        //#region Insert field
        //public int fbrno { get; set; }
        //public String FInterID { get; set; }
        //public String FTranType { get; set; }

        //public String FMRP { get; set; }
        //public String FType { get; set; }
        //public String FWorkShop { get; set; }
        //public String FItemID { get; set; }
        //public String FQty { get; set; }

        //public String FCommitQty { get; set; }

        //public String FConveyerID { get; set; }
        //public String FCheckerID { get; set; }
        //public String FBillerID { get; set; }
        //public String FSourceEntryID { get; set; }

        //public String FClosed { get; set; }
        //public String FNote { get; set; }
        //public String FUnitID { get; set; }
        //public String FAuxCommitQty { get; set; }
        //public String FAuxQty { get; set; }

        //public String FOrderInterID { get; set; }
        //public String FPPOrderInterID { get; set; }
        //public String FParentInterID { get; set; }

        //public String FSupplyID { get; set; }
        //public String FQtyFinish { get; set; }
        //public String FQtyScrap { get; set; }

        //public String FQtyForItem { get; set; }
        //public String FQtyLost { get; set; }
        //public String FRoutingID { get; set; }
        //public String FAuxQtyFinish { get; set; }

        //public String FAuxQtyScrap { get; set; }
        //public String FAuxQtyForItem { get; set; }
        //public String FAuxQtyLost { get; set; }
        //public String FMrpClosed { get; set; }

        //public String FBomInterID { get; set; }
        //public String FQtyPass { get; set; }
        //public String FAuxQtyPass { get; set; }
        //public String FQtyBack { get; set; }

        //public String FAuxQtyBack { get; set; }
        //public String FFinishTime { get; set; }
        //public String FReadyTIme { get; set; }
        //public String FPowerCutTime { get; set; }

        //public String FFixTime { get; set; }
        //public String FWaitItemTime { get; set; }
        //public String FWaitToolTime { get; set; }
        //public String FTaskID { get; set; }

        //public String FWorkTypeID { get; set; }
        //public String FCostObjID { get; set; }
        //public String FStockQty { get; set; }

        //public String FSuspend { get; set; }
        //public String FProjectNO { get; set; }
        //public String FProductionLineID { get; set; }
        //public String FReleasedQty { get; set; }

        //public String FReleasedAuxQty { get; set; }
        //public String FUnScheduledQty { get; set; }
        //public String FUnScheduledAuxQty { get; set; }

        //public String FSubEntryID { get; set; }
        //public String FScheduleID { get; set; }
        //public String FPlanOrderInterID { get; set; }
        //public String FProcessPrice { get; set; }

        //public String FProcessFee { get; set; }
        //public String FGMPCollectRate { get; set; }
        //public String FGMPItemBalance { get; set; }
        //public String FGMPBulkQty { get; set; }

        //public String FCustID { get; set; }
        //public String FMRPLockFlag { get; set; }
        //public String FHandworkClose { get; set; }
        //public String FConfirmerID { get; set; }

        //public String FInHighLimit { get; set; }
        //public String FInHighLimitQty { get; set; }
        //public String FAuxInHighLimitQty { get; set; }
        //public String FInLowLimit { get; set; }

        //public String FInLowLimitQty { get; set; }
        //public String FAuxInLowLimitQty { get; set; }
        //public String FChangeTimes { get; set; }
        //public String FCheckCommitQty { get; set; }

        //public String FAuxCheckCommitQty { get; set; }
        //public String FPlanConfirmed { get; set; }
        //public String FPlanMode { get; set; }
        //public String FMTONo { get; set; }

        //public String FPrintCount { get; set; }
        //public String FFinClosed { get; set; }
        //public String FFinCloseer { get; set; }
        //public String FStockFlag { get; set; }

        //public String FStartFlag { get; set; }
        //public String FVchInterID { get; set; }
        //public String FCardClosed { get; set; }
        //public String FHRReadyTime { get; set; }

        //public String FPlanCategory { get; set; }
        //public String FBomCategory { get; set; }
        //public String FSourceTranType { get; set; }
        //public String FSourceInterId { get; set; }

        //public String FReprocessedAuxQty { get; set; }
        //public String FReprocessedQty { get; set; }
        //public String FSelDiscardStockInAuxQty { get; set; }

        //public String FSelDiscardStockInQty { get; set; }
        //public String FDiscardStockInAuxQty { get; set; }
        //public String FDiscardStockInQty { get; set; }

        //public String FSampleBreakAuxQty { get; set; }
        //public String FSampleBreakQty { get; set; }
        //public String FResourceID { get; set; }
        //public String FAddInterID { get; set; }

        //public String FAPSImported { get; set; }
        //public String FAPSLastStatus { get; set; }
        //public String FAuxPropID { get; set; }
        //public String FOrderBOMEntryID { get; set; }

        //public String FIsMakeLowerBill { get; set; }
        //public String FConnectFlag { get; set; }


        //#endregion



    }
}
