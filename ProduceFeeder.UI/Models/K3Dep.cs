using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_Department")]
   public class K3Dep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("FItemID")]
        public int Id { get; set; }
        public string FName { get; set; }

        //public ICollection<ProduceItem> OutItems { get; set; }

    }
}
