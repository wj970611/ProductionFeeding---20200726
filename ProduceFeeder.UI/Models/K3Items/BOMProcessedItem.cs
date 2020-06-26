using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.K3Items
{
    /// <summary>
    /// 加工，处理件
    /// </summary>
    public class BOMProcessedItem : BOMItemBase
    {
  
        public string ProcessName { get; internal set; }
    }
}
