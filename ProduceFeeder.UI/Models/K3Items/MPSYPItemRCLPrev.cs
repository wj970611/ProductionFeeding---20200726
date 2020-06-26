using ProduceFeeder.UI.Models.YuPai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.K3Items
{
    /// <summary>
    ///热处理前 的工序投放跟踪
    /// </summary>
  public  class MPSYPItemRCLPrev
    { 
       
     
        public string CPFNumber { get; set; }
        public string FNumber { get; set; }
        public DateTime TLDate { get; set; }
        public MPSYPItemRCLPrevQty LN { get; set; }
        public MPSYPItemRCLPrevQty CJG { get; set; }
        public MPSYPItemRCLPrevQty DH { get; set; }
        /// <summary>
        /// 这里是标识出下一道工序热处理，已经准备好了。
        /// 
        /// </summary>
        public string NextGX { get; set; }
    }
   public class MPSYPItemRCLPrevQty
    {
        public int Qty { get; set; }
        public int StockQty { get; set; }

        public decimal RKL
        {
            get
            {
                var rkl= Qty == 0 ? 0 :  Math.Round((decimal) StockQty / Qty,2) * 100;
                return rkl;
            }
        }
    }

}
