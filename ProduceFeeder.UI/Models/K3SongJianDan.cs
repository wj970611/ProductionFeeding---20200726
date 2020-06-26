using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    [Table("t_BOS200000006")]
    public class K3SongJianDan
    {

        [Column("FID")]
        public int ID { get; set; }
        public string FBillNo { get; set; }
        public string FMultiCheckStatus { get; set; }
        public DateTime FDate { get; set; }

        public virtual ICollection<K3SongJianDanEntry> Entries { get; set; }



        public static decimal GetNoCheckItemQty(int itemId)
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            { 
                return (from s in _context.K3SongJianDans
                        join
                        n in _context.K3SongJianDanEntries on
                        s.ID equals n.FID
                        where s.FMultiCheckStatus != "16" && n.FItemID == itemId
                        select n.FQty - n.FQty1).DefaultIfEmpty(0).Sum();
            }
        }
        public async static Task<decimal> GetNoCheckItemQtyAsync(int itemId)
        {
            using (K3BhDBContext _context = new K3BhDBContext())
            { 
                return await (from s in _context.K3SongJianDans
                        join
                        n in _context.K3SongJianDanEntries on
                        s.ID equals n.FID
                        where s.FMultiCheckStatus != "16" && n.FItemID == itemId
                        select n.FQty - n.FQty1).DefaultIfEmpty(0).SumAsync();
            }
        }
    }
}
