using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Interface
{
    /// <summary>
    /// 由物料反算出可以出多少成品
    /// </summary>
  public  interface ICalCPQty
    {
        int CalCPQty();
    }
}
