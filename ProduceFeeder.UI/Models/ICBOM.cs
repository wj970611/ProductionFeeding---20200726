using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    /// <summary>
    /// BOM分解后的Item属性
    /// </summary>
    /// 
    [Table("vICBOM")]
    public class ICBOM : IDisposable
    {

        private DbContext _dbContext;
        public ICBOM()
        {

        }
        public ICBOM(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Key]
        public int FInterID { get; set; }
        public string FBOMNumber { get; set; }

        [Column("FItemID")]
        public int FatherItemId { get; set; }

        public string FNumber { get; set; }
        public string FStatus { get; set; }
        public string FUseStatus { get; set; }

        public string FUnitName { get; set; }

        public decimal FQty { get; set; }

        [NotMapped]
        public decimal FAuxQty { get; set; }

        public decimal FChildQty { get; set; }
        public decimal FScrap { get; set; }
        #region Child


        [ForeignKey("ProChildItem")]
        public int ChildItemID { get; set; }
        public K3Item ProChildItem { get; set; } 
        public string FChildNumber { get; set; }
        public string FChildName { get; set; }
        public int DepID { get; set; }
        #endregion







        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
