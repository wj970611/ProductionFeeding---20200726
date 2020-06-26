using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Models
{


    /// <summary>
    ///  配件类 采购
    /// </summary>
    public class BOMItemPurchase  
    {
 
   
        //public override decimal? InventoryQty => ICInventorys?.Select(x=>x.FQty).DefaultIfEmpty().Sum() + POInventorys?.Select(x=>x.FQty).DefaultIfEmpty().Sum() + IncomingNoCheckedQty;

        ///// <summary>
        ///// 来料未检的数量,此料未入实库和虚库
        ///// </summary>
        //public decimal IncomingNoCheckedQty { get; set; }

        //public decimal? ICInventoryQty => ICInventorys?.Select(x => x.FQty).DefaultIfEmpty().Sum();
        //public decimal? POInventoryQty => POInventorys?.Select(x => x.FQty).DefaultIfEmpty().Sum();


        ///// <summary>
        ///// 配装数，装成成品的数量
        ///// </summary>
        //public Int32? PZQty =>Convert.ToInt32((ICInventoryQty + POInventoryQty)/FBOMQty );



        ////public override decimal? RequiredQty => (PlanQty - (ICInventoryQty + POInventoryQty)) * FBOMQty;

        ///// <summary>
        ///// 虚库
        ///// </summary>
        //public List<POInventory> POInventorys { get; set; }


        
        
        //protected override decimal GetOutPutQtyAsync()
        //{
        //    var _inventoryQty = ICInventoryQty + POInventoryQty;
        //    ///获取预排表里的数量
        //    using (MyKingdeeDBContext _context=new MyKingdeeDBContext())
        //    {
        //         //_context.MPSYPFeedings.Where(x=>x.IsDeleted==false && x.)
        //    }
        //    return _inventoryQty.Value;
        //}
    }
}
