using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProduceFeeder.UI.Models
{

    [Table("cbCostObj")]
    public class K3CustObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("FItemID")]
        public int ID { get; set; }
        public string FNumber { get; set; }
    }
}