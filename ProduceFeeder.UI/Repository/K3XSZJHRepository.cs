using ProduceFeeder.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Repository
{
  public  class K3XSZJHRepository:IDisposable
    {

        K3BhDBContext _dbcontext = new K3BhDBContext();

        public void Dispose()
        {
            _dbcontext?.Dispose();
        }

        public IQueryable<K3XSZJHEntry> GetAll(int itemid)
        {
            return  _dbcontext.K3XSZJHEntries.Include("K3XSZJH").AsNoTracking()
                        .Where(w=>w.FItemID==itemid).Where(w=>w.K3XSZJH.FMultiCheckStatus=="16");

        }
    }
}
