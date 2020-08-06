using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.Repository
{
    class CustObjReposity  
    {
          
        public static int GetItemIdByFNumber(string fnumber)
        {
            K3BhDBContext _dbcontext = new K3BhDBContext();
            return _dbcontext.CustObjs.Where(x => x.FNumber == fnumber).Select(x=>x.ID).FirstOrDefault();
            
        }
      
    }
}
