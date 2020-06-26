using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.ItemsContainer;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 这个是一组投料计划
    /// 计划的功能：1、计算实时配件数量，得出是否满足投放。2、监控采购
    /// 3、投放前的物料计算
    /// </summary>
    /// 


    public class MPSPlanItem : INotifyPropertyChanged
    {
        [Key]
        public int ID { get; set; }
        public string BillNo { get; set; }
        public EnumRunningStatus RunningStatus { get; set; }
        public string Creator { get; set; }
        public int PlanTotalQty { get; set; } 
        public int PlanCount { get; set; }
        public int QJId { get; set; }
        public int K3ItemID => CPItem.K3ItemID;
        public string K3FNumber => CPItem.K3FNumber;

        public K3CPItemBase CPItem { get; set; } = new K3CPItemBase();
        /// <summary>
        /// 成品的制造车间
        /// </summary>
        public string CPDep { get; set; }
        [NotMapped]
        public WeekQty FirstWeek { get; set; }

        [NotMapped]
        public WeekQty SecWeek { get; set; }

        [NotMapped]
        public WeekQty ThirdWeek { get; set; }

        [NotMapped]
        public WeekQty FourWeek { get; set; }

        /// <summary>
        /// 第一阶段的调整数量，是指第一步骤选择计划的时候
        /// </summary>
        /// 
        private int planQty;
        public int PlanTLQty
        {
            get => planQty; set
            {
                //var maxvalue = PlanTotalQty - ICMOQty - TLQty - YPQty;
                //var minvalue = -YPQty;
                //if (maxvalue < value)
                //{
                //    planQty = maxvalue;

                //}
                //else if (minvalue > value)
                //{
                //    planQty = minvalue;
                //}
                //else
                //{
                //    planQty = value;
                //}
                //OnPropertyRaised("PlanTLQty");
                //OnPropertyRaised("RemainQty");
                planQty = value;
            }
        }

  


        #region 相关数量
        /// <summary>
        /// 已投数量
        /// </summary>
        /// 
        [NotMapped]
        public int ICMOQty { get => icmoQty; set { icmoQty = value; OnPropertyRaised("ICMOQty"); OnPropertyRaised("RemainQty"); OnPropertyRaised("ValidTlQty"); } }

        [NotMapped]
        public int YPQty { get => yPQty; set { yPQty = value; OnPropertyRaised("YPQty"); } }

        [NotMapped]

        public int TLQty { get => tLQty; set { tLQty = value; OnPropertyRaised("TLQty"); } }

        /// <summary>
        /// 在执行下一步操作之前，判断该类数量的合理性
        /// </summary>
        public bool IsValidQty => remainQty >= 0 && planQty + YPQty > 0;
        private int remainQty;
        private int icmoQty;
        private int yPQty;
        private int tLQty;

        public int RemainQty
        {
            get
            {
                remainQty = PlanTotalQty - ICMOQty - TLQty - YPQty - planQty;
                return remainQty;
            }
        }
        /// <summary>
        /// 未完成数量；计划总数-入库
        /// </summary>
        public int NotFinishQty => PlanTotalQty - icmoQty;
        public int ValidTlQty
        {
            get
            {
                return PlanTotalQty - icmoQty - TLQty;
            }
        }

        /// <summary>
        /// 获取没有执行的计划，也就是还要执行的计划
        /// 计划-预排-待投-任务单
        /// </summary>
        public void GetQty(QJItem qjItem)
        {
            //获取已投的数量，除了作废的所有数量
            var firstweek = qjItem.StartDate.AddDays(7);
            var secweek = qjItem.StartDate.AddDays(14);
            var thirdweek = qjItem.StartDate.AddDays(21);

            ICMOQty = (int)new ICMORepository().GetAllCP().Where(w => w.FCancellation == false)
                        .Where(w => w.FItemId == K3ItemID).Where(w => w.QJId == QJId)
                        .Select(s => s.FauxQty).DefaultIfEmpty(0).Sum();
            //获取预排数量
            YPQty = (int)new Repository.MPSYPItemRepository().GetALL().Where(w => w.CPItem.K3ItemID == K3ItemID).Select(s => s.OnePlanQTY).DefaultIfEmpty(0).Sum();
            TLQty = (int)  MPSTLContainer.WaittingTLQtyBYCPFNumber(K3FNumber);

            FirstWeek = new WeekQty
            {
                ZJHQty = (int)new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= qjItem.StartDate && x.FDate < firstweek)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).Sum(),
                ZTLQty = (int)new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= qjItem.StartDate && x.FCommitDate < firstweek)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).Sum()
            };
            SecWeek = new WeekQty
            {
                ZJHQty = (int)new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= firstweek && x.FDate < secweek)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).Sum(),
                ZTLQty = (int)new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= firstweek && x.FCommitDate < secweek)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).Sum()
            };
            ThirdWeek = new WeekQty
            {
                ZJHQty = (int)new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= secweek && x.FDate < thirdweek)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).Sum(),
                ZTLQty = (int)new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= secweek && x.FCommitDate < thirdweek)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).Sum()
            };
            FourWeek = new WeekQty
            {
                ZJHQty = (int)new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= thirdweek && x.FDate <= qjItem.EndDate)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).Sum(),
                ZTLQty = (int)new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= thirdweek && x.FCommitDate <= qjItem.EndDate)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).Sum()
            };
        }
        public decimal ZJHQty { get; set; }

        internal async Task GetQtyAsync(QJItem qjItem)
        {
            var firstweek = qjItem.StartDate.AddDays(7);
            var secweek = qjItem.StartDate.AddDays(14);
            var thirdweek = qjItem.StartDate.AddDays(21);


            //获取已投的数量，除了作废的所有数量
            ICMOQty = (int)await new ICMORepository().GetAllCP().Where(w => w.FCancellation == false)
                                                  .Where(w => w.FItemId == K3ItemID).Where(w => w.QJId == qjItem.FItemId)
                                                  .Select(s => s.FauxQty).DefaultIfEmpty(0).SumAsync();
            //获取预排数量
            YPQty = (int)await new Repository.MPSYPItemRepository().GetALL().Where(w => w.CPItem.K3ItemID == K3ItemID).Select(s => s.Qty).DefaultIfEmpty(0).SumAsync();

            TLQty = (int)await MPSTLContainer.WaittingTLQtyBYCPFNumberAsync(K3FNumber);

            FirstWeek = new WeekQty
            {
                ZJHQty = (int)await new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= qjItem.StartDate && x.FDate < firstweek)
                                  .Select(s => s.FQty).DefaultIfEmpty(0).SumAsync(),
                ZTLQty = (int)await new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= qjItem.StartDate && x.FCommitDate < firstweek)
                                  .Select(s => s.FauxQty).DefaultIfEmpty(0).SumAsync()
            };
            SecWeek = new WeekQty
            {
                ZJHQty = (int)await new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= firstweek && x.FDate < secweek)
                                  .Select(s => s.FQty).DefaultIfEmpty(0).SumAsync(),
                ZTLQty = (int)await new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= firstweek && x.FCommitDate < secweek)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).SumAsync()
            };
            ThirdWeek = new WeekQty
            {
                ZJHQty = (int)await new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= secweek && x.FDate < thirdweek)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).SumAsync(),
                ZTLQty = (int)await new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= secweek && x.FCommitDate < thirdweek)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).SumAsync()
            };
            FourWeek = new WeekQty
            {
                ZJHQty = (int)await new Repository.K3XSZJHRepository().GetAll(K3ItemID).Where(x => x.FDate >= thirdweek && x.FDate <= qjItem.EndDate)
                                    .Select(s => s.FQty).DefaultIfEmpty(0).SumAsync(),
                ZTLQty = (int)await new ICMORepository().GetItem(K3ItemID).Where(x => x.FCommitDate >= thirdweek && x.FCommitDate <= qjItem.EndDate)
                                    .Select(s => s.FauxQty).DefaultIfEmpty(0).SumAsync()
            };
        }



        #endregion


        #region 预测数量


        /// <summary>
        /// 成品的物料可投数量。由库存进行反算。
        /// =物料子项的可用数量+预排数量。
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>  
        public void GetProdceYCQty()
        {
            //var  _ypQty = (int)new Repository.MPSYPItemRepository().GetQty(CPItem.K3ItemID);

            //List<BOMItemBase> cpItems = CPItem.GetComponetItemsConsum(_ypQty);

            CPItem.ComponentItems.ForEach(f =>
            {
                //把预排表里的消耗与可用库存相加，得出的是实际可投的物料子项
                f.ValidTLQty = f.ItemUseableQty;
            });

            //选择配件的最小配数 得出最大的配装数
            //在计算原料的时候（精车件），考虑在制数量
            MaxOutputQty = (int)CPItem.ComponentItems.Select(s => s.ItemOutputQty).DefaultIfEmpty(0).Max();
        }

        private int maxOutputQty;
        private int planTLQty;

        public int MaxOutputQty
        {
            get { return maxOutputQty; }
            set { maxOutputQty = value; OnPropertyRaised("MaxOutputQty"); }
        }

        /// <summary>
        /// 最小的投料计算，是投料数-在制品
        /// 
        /// </summary>
        public int MinRawItemTL { get; set; }
        #endregion





















        #region Method

    
        /// <summary>
        /// 计划转投料
        /// </summary>
        /// <returns></returns>

        public  MPSTLItem  ToTLItem()
        {
            return new MPSTLItem
            { 
                OnePlanQTY=planQty,
                PlanStamp=DateTime.Now.ToString(),
                QJId=QJId,
                CPItem = CPItem,
            };
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
