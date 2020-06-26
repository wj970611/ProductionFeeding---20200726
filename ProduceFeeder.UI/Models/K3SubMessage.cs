using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_submessage")]
  public  class K3SubMessage
    {
        [Column("FinterID")]
        public int ID { get; set; }

        public string FName { get; set; }

    }
}
