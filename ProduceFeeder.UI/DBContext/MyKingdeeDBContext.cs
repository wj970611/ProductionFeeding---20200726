using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai; 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI
{
  public  class MyKingdeeDBContext : DbContext
    {
        public MyKingdeeDBContext()
            : base("name=MyKingdeeDBConnstring")
        {

            
        } 
        public DbSet<MPSYPItem> MPSYPFeedings { get; set; }
        public DbSet<RclMachine> RclMachines { get; set; }
        public DbSet<MPSTLItem> MPSTLItems { get; set; }
        public DbSet<MPSPlanItem> MPSTLPlanItems { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.ComplexType<K3Dep>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
