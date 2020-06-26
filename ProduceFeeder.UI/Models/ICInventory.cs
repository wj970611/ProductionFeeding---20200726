using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    /// <summary>
    /// 金蝶实库
    /// </summary> 
    /// 
 [Table("ICInventory")]
  public  class ICInventory
    {
        [Column("FItemId")]
        public int Id { get; set; }
        public string FBatchNo { get; set; }
        public decimal FQty { get; set; }

        public int FStockID { get; set; }

        [ForeignKey("K3AuxItem")]
        public int FAuxPropID { get; set; }
        public K3AuxItem K3AuxItem { get; set; }

        public static async Task<List<ICInventory>> GetICInventoriesAsync(int itemId)
        {
            using (K3BhDBContext _context=new K3BhDBContext())
            { 
               return await _context.Set<ICInventory>().
                           Include("K3AuxItem").Where(x => x.Id == itemId && x.FQty>0).ToListAsync();
            }
        }
        public static  List<ICInventory>  GetICInventories(int itemId)
        {
            using (K3BhDBContext _context=new K3BhDBContext())
            {

               return   _context.Set<ICInventory>().
                           Include("K3AuxItem").Where(x => x.Id == itemId && x.FQty>0).ToList();
            }
        }

    }
}

