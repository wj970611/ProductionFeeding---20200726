using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{

    public struct ItemProcess
    {
        public string Name { get; set; }
        public int PlanQty { get; set; }

        public int RkQty { get; set; }
        public decimal FinishedRate => Math.Round((decimal)RkQty / PlanQty, 2);
    }
    class ICMORuning
    {

        public ItemProcess LN { get; set; }
        public ItemProcess CJG { get; set; }

        public ItemProcess DH { get; set; }
        public ItemProcess PM { get; set; }
        public ItemProcess WX { get; set; }
        public ItemProcess MJG { get; set; }
        public ItemProcess ZP { get; set; }

    }
}
