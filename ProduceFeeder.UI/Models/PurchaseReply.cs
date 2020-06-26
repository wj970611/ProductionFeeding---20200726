using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 采购回复表
    /// </summary>
    /// 
    [Table("PurchaseReply")]
   public class PurchaseReply
    {

        public BOMItemPurchase PurchaseItem { get; set; }

        public POOrder POOrder { get; set; }

        public DateTime? ReplyDate { get; set; }


    }
}
