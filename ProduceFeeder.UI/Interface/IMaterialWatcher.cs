using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models.YuPai
{
    /// <summary>
    /// 物料监控器，监控物料不足时 发出警告
    /// </summary>
   public  interface IMaterialWatcher
    {
        /// <summary>
        /// 当前数量是否溢出。由于计划的减少，造成原来投入的超出
        /// 从预排还是减少，再减投料
        /// </summary>
        //bool IsOver { get; set; }
    }
}
