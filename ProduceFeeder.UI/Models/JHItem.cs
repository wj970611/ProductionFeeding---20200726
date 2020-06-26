using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{
    /// <summary>
    /// 生产计划下的物料类
    /// </summary>
  public  class JHItem
    {

        public ItemBaseInfo ItemBaseInfo { get; set; }

        //计划数量
        public decimal JHFQty { get; set; }


        //预配数量
        public decimal YPFQty { get; set; }


    }
}
 