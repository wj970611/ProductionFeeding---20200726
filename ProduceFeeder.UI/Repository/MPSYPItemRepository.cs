using ProduceFeeder.UI.Models;
using System;
using System.Linq;

namespace ProduceFeeder.UI.Repository
{
    public class MPSYPItemRepository : IDisposable
    {
        MyKingdeeDBContext _dbcontext = new MyKingdeeDBContext();


        public decimal GetQty(int itemId)
        {
            return _dbcontext.MPSYPFeedings.AsNoTracking().Where(x => x.CPItem.K3ItemID == itemId).Select(s => s.OnePlanQTY).DefaultIfEmpty(0).Sum();
        }

        public IQueryable<MPSYPItem> GetALL()
        {
            return _dbcontext.MPSYPFeedings.Include("CPItem").Where(w => w.IsDeleted == false);

        }
        public int Insert(MPSYPItem item)
        {
            int _id=_dbcontext.MPSYPFeedings.Select(x=>x.ID).DefaultIfEmpty(0).Max();
            var _billno = "YP" +  (_id + 1).ToString().PadLeft(6,'0');
            item.BillNo = _billno;
            _dbcontext.MPSYPFeedings.Add(item);
            return _dbcontext.SaveChanges();
        }


        public int Update(MPSYPItem item)
        {
            _dbcontext.Entry(item).State = System.Data.Entity.EntityState.Modified;
            return _dbcontext.SaveChanges();
        }


        public int Delete(MPSYPItem item)
        {
            var _item= _dbcontext.Set<MPSYPItem>().Find(item.ID); 
            _item.IsDeleted = true;
            _dbcontext.Entry(_item).State = System.Data.Entity.EntityState.Modified;
            return _dbcontext.SaveChanges();
        }


        public void Dispose()
        {
            _dbcontext?.Dispose();
        }


    }
}
