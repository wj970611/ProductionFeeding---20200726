using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// K3辅助类
    /// </summary>
    /// 
    [Table("t_AuxItem")]
  public  class K3AuxItem
    {
        [Key]
        [Column("FItemID")]
        public int Id { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }

    }
}
