namespace ProduceFeeder.UI
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ProduceFeeder.UI.Models;
    using ProduceFeeder.UI.Models.YuPai;

    public partial class K3BhDBContext : DbContext
    {
        public K3BhDBContext()
            : base("name=K3DBConnstring")
        {
            Database.SetInitializer<K3BhDBContext>(null);
        }

        public DbSet<K3Item> K3Items { get; set; }
        public DbSet<POInventory> POInventories { get; set; }
        public DbSet<ICInventory> ICInventories { get; set; }
        public DbSet<ICMO> ICMOs { get; set; }
        public DbSet<QJItem> QJItems { get; set; }
        public DbSet<K3CustObj> CustObjs { get; set; }
        public DbSet<K3SongJianDanEntry> K3SongJianDanEntries { get; set; }
        public DbSet<K3SongJianDan> K3SongJianDans { get; set; }
        public DbSet<K3Dep> K3Deps { get; set; }
        public DbSet<PPBOMEntry> PPBOMEntrys { get; set; }
        public DbSet<K3XSZJH> K3XSZJHs { get; set; }
        public DbSet<K3XSZJHEntry> K3XSZJHEntries { get; set; }
        public DbSet<K3MPSEntry> K3Entries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<POInventory>()
                .HasKey(c => new { c.Id, c.FBatchNo, c.FStockID,c.FAuxPropID  });
            modelBuilder.Entity<ICInventory>()
                .HasKey(c => new { c.Id, c.FBatchNo, c.FStockID, c.FAuxPropID });
            modelBuilder.Entity<ICBOM>().HasKey(c => new { c.FatherItemId, c.ChildItemID });

            modelBuilder.Entity<PPBOMEntry>()
                .HasKey(k => new { k.FInterID, k.FEntryID });
            //modelBuilder.Entity<ICMO>()
            //    .HasMany(m => m.PPBOMEntry)
            //    .WithRequired()
            //    .HasForeignKey(k => k.FICMOInterID);
            base.OnModelCreating(modelBuilder); 
        }
    }
}
