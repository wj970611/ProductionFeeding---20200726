using ProduceFeeder.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Repository
{
   public class K3MPSRepository
    {

        K3BhDBContext _dbcontext = new K3BhDBContext();
        public IQueryable<K3MPSEntry> GetAll()
        {
           return _dbcontext.K3Entries.Where(w=>new string[] { "1", "2", "4" }.Contains(w.K3MPS.FComboBox)) 
                ;
        }
    }
}
