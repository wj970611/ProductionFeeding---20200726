using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("PPBOMEntry")]
   public class PPBOMEntry
    {
        public int FInterID { get; set; }
        public int FEntryID { get; set; }
        public int FItemID { get; set; }

        /// <summary>
        /// 应发数量
        /// </summary>
        public decimal FQtyMust { get; set; }
        /// <summary>
        /// 实发数量
        /// </summary>
        public decimal FAuxQty { get; set; }
        [ForeignKey("FICMOInterID")]
        public ICMO ICMO { get; set; }
        public int FICMOInterID { get; set; }
    }
}
