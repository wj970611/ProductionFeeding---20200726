using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Interface
{
    public interface ICalUseQty
    {

        /// <summary>
        /// 由计算得出的最大数量
        /// </summary>
        int MaxUseableQty { get; set; }
        /// <summary>
        /// 记录一次调整后的可以用数量，用来对比发现计划是否更改
        /// </summary>
        decimal CanUseQty { get;  }

        decimal GetCanUseQty();
    }
}
