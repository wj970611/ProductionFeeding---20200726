using ProduceFeeder.UI.Interface;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models 
{
    public enum FeedingType
    {
        计划,
        配料
    }
    /// <summary>
    /// 异步工序状态
    /// </summary>

    public enum EnumRunningStatus
    {
        工序异步,
        等待同步,
        等待投料,
        完成
    }
    /// <summary>
    /// 预投表,预投表里要解决的主要问题是，预投的数量。
    /// 物料有没有到期，工序是否一致，计划有没有更改？
    /// </summary>
    //[Table("wjj_MPSYPItem")]
    public class MPSYPItem : MPSItemBase, IProduceYCQty
    {
        private EnumRunningStatus runningStatus; 
 
        /// <summary>
        /// 等待，等待同步，同步，完成
        /// </summary>
        public EnumRunningStatus RunningStatus { get => runningStatus; set { runningStatus = value; OnPropertyRaised("RunningStatus"); } }


        /// <summary>
        /// 监控物料运行时的同步状态,如果投放的先期物料已经入库，则都
        /// 出在工序热处理同步状态
        /// </summary>
        //public bool RunningProcessSync { get; set; }

        /// <summary>
        /// 判断是否可以预投任务单
        /// 1、必须是自制产品
        /// 2、存货数量不能满足投料
        /// </summary>
        public bool CanYTL
        {
            get { return this.CPItem.CPItemType == CPItemType.自制; }
        }


        //类型为 投平 计划
        public string FeedingType { get; set; }

         


        public string SourceBillNo { get; set; }
         

        ///如何判断成品是个异步工序
        ///也就是热处理前的大小圈的工序是不一样的
        ///方法1 看各自套圈的工序是不是个数相同
        ///方法2 找出热处理前的工序，看是不是精车件。


        /// <summary>
        /// 由计算得出的库存最大数量
        /// </summary>
        public int MaxUseableQty { get; set; }
         
        //public ICollection<MPSTLPlanItem> MPSTLPlanItems { get; set; }


        public int MaxOutputQty { get; set; } 

        /// <summary>
        /// 记录一次调整后的可以用数量，用来对比发现计划是否更改
        /// </summary> 



        ///要提供对一下icmo的关注。
        ///
        /// 
        /// 


        ///预排里的单子还要跟踪计划的变化
        ///
        private void TrackJH() { }



        ///1、判断需要提前下的依据。


        /// <summary>
        /// 下投任务单
        /// </summary>
        public void ExecTL()
        {
            //分解热处理之前的工序。
            //var rings=Rings.Where(r => r.RCLFirst == true);

        }

        /// <summary>
        /// 检测能不能下投 
        /// 1、产品要复合投料不同步。
        /// 2、已经下达的不能重复下。
        /// </summary>
        /// <returns></returns>
        public bool CanExecTL()
        {

            return false;

        }

        /// <summary>
        /// 下投到排料
        /// </summary>
        public void ExecTLSchedule() { }

        /// <summary>
        /// 获取预投的items,
        /// </summary>
        /// <returns></returns>
        public List<MPSYPItemRCLPrev> GetYT()
        {
            List<MPSYPItemRCLPrev> lst = new List<MPSYPItemRCLPrev>();  
            var qq = new Repository.ICMORepository().GetAll().Where(x => x.myBillNo ==BillNo).ToList();
            foreach (var item in qq)
            {
                lst.Add(new MPSYPItemRCLPrev()
                {
                    CPFNumber =CPItem.K3FNumber,
                    FNumber = item.FItem.FNumber,
                    TLDate = item.FPlanCommitDate.Value,
                    LN = new MPSYPItemRCLPrevQty
                    {
                        Qty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.19")).Select(x => x.FauxQty).DefaultIfEmpty(0).Sum(),
                        StockQty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.19")).Select(x => x.FAuxStockQty).DefaultIfEmpty(0).Sum()
                    },
                    CJG = new MPSYPItemRCLPrevQty
                    {
                        Qty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.20")).Select(x => x.FauxQty).DefaultIfEmpty(0).Sum(),
                        StockQty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.20")).Select(x => x.FAuxStockQty).DefaultIfEmpty(0).Sum()
                    },
                    DH = new MPSYPItemRCLPrevQty
                    {
                        Qty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.21")).Select(x => x.FauxQty).DefaultIfEmpty(0).Sum(),
                        StockQty = (int)qq.Where(x => x.FItem.FNumber.StartsWith("5.21")).Select(x => x.FAuxStockQty).DefaultIfEmpty(0).Sum()
                    },
                });

            }
            return lst;
        }

        public void GetProdceYCQty()
        {
            throw new NotImplementedException();
        }
    }
}
