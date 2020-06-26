namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department");
            DropIndex("dbo.K3CPItemBase", new[] { "K3Dep_Id" });
            AddColumn("dbo.K3CPItemBase", "DepFName", c => c.String());
            AddColumn("dbo.K3CPItemBase", "DepId", c => c.Int(nullable: false));
            DropColumn("dbo.K3CPItemBase", "K3Dep_Id");
            DropTable("dbo.t_Department");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.t_Department",
                c => new
                    {
                        FItemID = c.Int(nullable: false),
                        FName = c.String(),
                    })
                .PrimaryKey(t => t.FItemID);
            
            AddColumn("dbo.K3CPItemBase", "K3Dep_Id", c => c.Int());
            DropColumn("dbo.K3CPItemBase", "DepId");
            DropColumn("dbo.K3CPItemBase", "DepFName");
            CreateIndex("dbo.K3CPItemBase", "K3Dep_Id");
            AddForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department", "FItemID");
        }
    }
}
