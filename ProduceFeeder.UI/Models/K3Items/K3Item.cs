using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    public struct ItemBaseInfo
    {

        public int FItemID { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public int FDeleted { get; set; }
    }



    [Table("t_ICItem")]
    public class K3Item
    {
        [Key]  
        [Column("FItemID")]
        public int   ID { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public short FDeleted { get; set; }


        public string FModel { get; set; }

        /// <summary>
        /// 安全库存
        /// </summary>
        public decimal FSecInv { get; set; }


        /// <summary>
        /// 生产车间
        /// </summary>
        /// 


        [ForeignKey("WorkShop")]
        [Column("FSource")]
        public int? WorkShopId { get; set; }
        public K3Dep WorkShop { get; set; }


        //[ForeignKey("WorkShop")]
        //[Column("F_104")]
        //public int? WorkShopId { get; set; }
        //public K3Dep WorkShop { get; set; }

        ///// <summary>
        ///// 发货车间
        ///// </summary>
        //[Column("F_105")]
        //[ForeignKey("FHWorkShop")]
        //public int? FHId { get; set; }
        //public K3Dep FHWorkShop { get; set; }

        ///// <summary>
        ///// 自制 外购 委外 虚拟件
        ///// </summary>
        ///// 
        //[ForeignKey("K3SubMessage")]
        //[Column("FErpClsID")]

        //public int K3SubMessageID { get; set; }

        //public K3SubMessage K3SubMessage { get; set; }



    }
}


