using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Interface
{

    /// <summary>
    /// 产品预测接口
    /// </summary>
  public  interface IProduceYCQty
    {
         int MaxOutputQty { get; set; }
        void GetProdceYCQty();
    }
}
