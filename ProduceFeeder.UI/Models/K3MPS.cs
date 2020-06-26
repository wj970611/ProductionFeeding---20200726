using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_zscjhmxqd")]
   public class K3MPS
    {
        [Key]
        public int FID { get; set; }

        /// <summary>
        /// 4手工调整 3=投放生产计划单，2=销售计划调整 1=主生产计划
        /// </summary>
        public string FComboBox { get; set; }

   
         [Column("FDate")]
        public DateTime FPlanBeginDate { get; set; }

        //public int FPlanQty => (int)Entries?.Where(w=>w.FID==FID).Select(s => s.FQty).DefaultIfEmpty(0).Sum();
        public virtual ICollection<K3MPSEntry> Entries { get; set; }

    }
    [Table("t_zscjhmxqdEntry2")]
   public class K3MPSEntry
    {
        [Column("FEntryID")]
        public int ID { get; set; }
        public int FID { get; set; }

        [ForeignKey("K3Item")]
        [Column("FBase")]
        public int FItemID { get; set; }
        public K3Item K3Item { get; set; }

        [Column("FQty")]
        public decimal FQty { get; set; }



        [Column("FBase3")]
        [ForeignKey("K3Dep")]
        public int DepId { get; set; }
        public K3Dep K3Dep { get; set; }

        [Column("FBase2")]
        [ForeignKey("QJItem")]
        public int QJId { get; set; }
        public QJItem QJItem { get; set; }
 
        public K3MPS K3MPS { get; set; }
    }
}
