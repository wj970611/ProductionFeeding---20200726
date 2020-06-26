using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_xszjhEntry2")]
  public  class K3XSZJHEntry
    {
        [Key]
        public int FEntryID { get; set; }
        [Column("FBase2")]
        public int FItemID { get; set; }

        [ForeignKey("K3XSZJH")]
        public int FID { get; set; }
        public K3XSZJH K3XSZJH { get; set; }

        public decimal FQty { get; set; }
        public DateTime FDate { get; set; }
    }
}
