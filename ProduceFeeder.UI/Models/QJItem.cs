using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    [Table("t_Item_3019")]
   public class QJItem
    {
        [Key]
        public int FItemId { get; set; }
        [Column("F_101")]
        public DateTime StartDate { get; set; }
        [Column("F_102")]
        public DateTime EndDate { get; set; }
        //例201808
        public string FNumber { get; set; }

        //例2018年8月
        public string FName { get; set; }
        public int CurYear => StartDate.Year;
        public int CurMon => StartDate.Month;




        ///安全库存
        ///

    }
}
