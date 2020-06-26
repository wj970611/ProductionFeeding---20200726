using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_xszjh")]
   public class K3XSZJH
    {
        [Key]
        public int FID { get; set; }
        public string FBillNo { get; set; }
        [Column("FBase4")]
        public int FQJID { get; set; }
        public string FMultiCheckStatus { get; set; }
        public ICollection<K3XSZJHEntry> Entries { get; set; }
    }
}
