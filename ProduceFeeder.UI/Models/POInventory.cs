using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 金蝶虚库
    /// </summary>
    /// 
    [Table("poInventory")]
  public  class POInventory
    {
        [Column("FItemId")]
        public int Id { get; set; }
        public string FBatchNo { get; set; }
        public decimal FQty { get; set; }

        public int FStockID { get; set; }


        [ForeignKey("K3AuxItem")]
        public int FAuxPropID { get; set; }
        public K3AuxItem K3AuxItem { get; set; }

        internal static List<POInventory> GetPOInventories(int itemId)
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            { 
                return _context.Set<POInventory>().
                            Include("K3AuxItem").Where(x => x.Id == itemId && x.FQty>0).ToList();
            }
        }
        internal async static Task<List<POInventory>> GetPOInventoriesAsync(int itemId)
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            { 
                return await _context.Set<POInventory>().
                            Include("K3AuxItem").Where(x => x.Id == itemId && x.FQty>0).ToListAsync();
            }
        }
    }
}
