using ProduceFeeder.UI.Interface;
using ProduceFeeder.UI.Models.K3Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 投料的处理状态，正常，加急，迟缓
    /// </summary>
    public enum TLhandleStatus
    {
        Normal,
        Urgent,
        Delay
    }

    public enum TLRunningStatus
    {
        等待,
        淬火中,
        完成
    }
    /// <summary>
    /// 热处理投料表
    /// </summary>


    //[Table("wjj_TLSchedule")]
    public class MPSTLItem : MPSItemBase
    {



        private string rclLine="新线1";
        private int xH;
        /// <summary>
        /// 根据处理状态的不同显示不同的图片
        /// </summary>
        /// 
        [MaxLength(30)]
        public string IconPath { get => iconPath; set { iconPath = value; OnPropertyRaised("IconPath"); } }
        public TLhandleStatus HandleStatus
        {
            get => handleStatus; set
            {
                handleStatus = value;

                switch (handleStatus)
                {
                    case TLhandleStatus.Normal:
                        IconPath = "/image/flag_blue.png";
                        break;
                    case TLhandleStatus.Urgent:
                        IconPath = "/image/flag_red.png";
                        break;
                    case TLhandleStatus.Delay:
                        IconPath = "/image/flag_yellow.png";
                        break;
                    default:
                        IconPath = "/image/flag_blue.png";
                        break;
                }
            }
        }  

        /// <summary>
        /// 序号
        /// </summary>
        public int XH { get => xH; set { xH = value;OnPropertyRaised("XH"); } }

        public string LuCode { get; set; }
        public string PCH { get; set; }
        public string LuCode2 { get; set; }


        public string FSourceBillNo { get; set; }
        public int FSourceInnerID { get; set; }
        /// <summary>
        /// 待投 投料中 已投
        /// </summary>
        public TLRunningStatus RunningStatus { get => runningStatus; set { runningStatus = value; OnPropertyRaised("RunningStatus"); } }

        /// <summary>
        /// 要求投料日期
        /// </summary>
        private DateTime orderDate = DateTime.Now.Date;

        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                orderDate = value;
                OnPropertyRaised("OrderDate");
            }
        }
        /// <summary>
        /// 投料日期
        /// </summary>
        /// 
        private DateTime? feedingDate;
        public DateTime? FeedingDate
        {
            get { return feedingDate; }
            set
            {
                feedingDate = value;
                if (value != null) RunningStatus = TLRunningStatus.淬火中;
                OnPropertyRaised("FeedingDate");
            }
        }

        /// <summary>
        /// 出料日期
        /// </summary>
        /// 
        private DateTime? outFeedingDate;
        public DateTime? OutFeedingDate
        {
            get { return outFeedingDate; }
            set
            {
                outFeedingDate = value;
                if (value != null) RunningStatus = TLRunningStatus.完成;
                OnPropertyRaised("OutFeedingDate");
            }
        }
        private string iconPath = @"/image/flag_blue.png";

        private TLhandleStatus handleStatus;
        private TLRunningStatus runningStatus;

        /// <summary>
        /// 炉线号
        /// </summary>

        public string RclLine
        {
            get { return rclLine; }
            set { rclLine = value; }
        }
         
        /// <summary>
        /// 类型为 投平 计划
        /// </summary>
        public FeedingType FeedingType { get; set; }


        /// <summary>
        /// 生产备注
        /// </summary>
        public string Remark { get; set; } 

    


    }
}
