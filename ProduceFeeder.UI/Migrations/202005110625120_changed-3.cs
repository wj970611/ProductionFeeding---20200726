namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department");
            DropPrimaryKey("dbo.t_Department");
            AlterColumn("dbo.t_Department", "FItemID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.t_Department", "FItemID");
            AddForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department", "FItemID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department");
            DropPrimaryKey("dbo.t_Department");
            AlterColumn("dbo.t_Department", "FItemID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.t_Department", "FItemID");
            AddForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department", "FItemID");
        }
    }
}
