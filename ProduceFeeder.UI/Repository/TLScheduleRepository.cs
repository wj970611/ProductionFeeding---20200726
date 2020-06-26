using ProduceFeeder.UI.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Repository
{
    /// <summary>
    /// 投料表
    /// </summary>
    internal class TLScheduleRepository :IDisposable
    {
        MyKingdeeDBContext _dbcontext = new MyKingdeeDBContext();


        public IQueryable<MPSTLItem> GetALL()
        {
            return _dbcontext.MPSTLItems.Include("CPItem").Where(w => w.IsDeleted == false); 
        }
      
        public int Update(MPSTLItem item)
        {
            if (item.SubFItemId == 0) return 0;
            _dbcontext.Entry(item).State = System.Data.Entity.EntityState.Modified;
            return _dbcontext.SaveChanges();
        }
        public int Insert(MPSTLItem item)
        {
            if (item.SubFItemId == 0) return 0;
            int _id = _dbcontext.MPSTLItems.Select(x => x.ID).DefaultIfEmpty(0).Max();
            int _xh = _dbcontext.MPSTLItems.Select(x => x.XH).DefaultIfEmpty(0).Max();
            var _billno = "TL" + (_id + 1).ToString().PadLeft(6, '0');

            item.BillNo = _billno;
            item.XH = _xh+1;
            item.IsDeleted = false;
            _dbcontext.MPSTLItems.Add(item);
            return _dbcontext.SaveChanges();
        }
        /// <summary>
        /// 用户权下的删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int UserDelete(MPSTLItem item)
        {
            if (item ==null || item.SubFItemId==0 || item.FeedingDate !=null || item.OutFeedingDate !=null)
            {
                return 0;
            }
            item.IsDeleted = true;
            _dbcontext.Entry(item).State = EntityState.Modified; 
            return  _dbcontext.SaveChanges();
        }

  


        public void Dispose()
        {
            _dbcontext?.Dispose();
        }
    }
}
