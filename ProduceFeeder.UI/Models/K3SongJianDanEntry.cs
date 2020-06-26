using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// K3的送检单,运用此单，要求及时处理此单
    /// </summary>
    /// 
    [Table("t_BOS200000006entry2")]
   public class K3SongJianDanEntry
    {
        [Column("FEntryID")]
        public int ID { get; set; }

        [ForeignKey("K3SongJianDan")]
        public int FID { get; set; }
        public K3SongJianDan K3SongJianDan { get; set; }

        [ForeignKey("K3Item")]
        [Column("FBase1")] 
        public int FItemID { get; set; }
        public K3Item K3Item { get; set; }

        public decimal FQty { get; set; }
        public decimal FQty1 { get; set; }
    }
}
