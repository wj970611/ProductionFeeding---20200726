namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.t_ICItem", "FSource", "dbo.t_Department");
            DropForeignKey("dbo.MPSTLItems", "SubItem_ID", "dbo.t_ICItem");
            DropForeignKey("dbo.MPSYPItems", "SubItem_ID", "dbo.t_ICItem");
            DropIndex("dbo.MPSTLItems", new[] { "SubItem_ID" });
            DropIndex("dbo.t_ICItem", new[] { "FSource" });
            DropIndex("dbo.MPSYPItems", new[] { "SubItem_ID" });
            AddColumn("dbo.MPSTLItems", "SubFNumber", c => c.String());
            AddColumn("dbo.MPSTLItems", "SubFName", c => c.String());
            AddColumn("dbo.MPSTLItems", "SubFModel", c => c.String());
            AddColumn("dbo.MPSTLItems", "SubFItemId", c => c.Int(nullable: false));
            AddColumn("dbo.MPSYPItems", "SubFNumber", c => c.String());
            AddColumn("dbo.MPSYPItems", "SubFName", c => c.String());
            AddColumn("dbo.MPSYPItems", "SubFModel", c => c.String());
            AddColumn("dbo.MPSYPItems", "SubFItemId", c => c.Int(nullable: false));
            DropColumn("dbo.MPSTLItems", "SubItem_ID");
            DropColumn("dbo.MPSYPItems", "SubItem_ID");
            DropTable("dbo.t_ICItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.t_ICItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FItemID = c.Int(nullable: false),
                        FNumber = c.String(),
                        FName = c.String(),
                        FDeleted = c.Short(nullable: false),
                        FModel = c.String(),
                        FSecInv = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FSource = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.MPSYPItems", "SubItem_ID", c => c.Int());
            AddColumn("dbo.MPSTLItems", "SubItem_ID", c => c.Int());
            DropColumn("dbo.MPSYPItems", "SubFItemId");
            DropColumn("dbo.MPSYPItems", "SubFModel");
            DropColumn("dbo.MPSYPItems", "SubFName");
            DropColumn("dbo.MPSYPItems", "SubFNumber");
            DropColumn("dbo.MPSTLItems", "SubFItemId");
            DropColumn("dbo.MPSTLItems", "SubFModel");
            DropColumn("dbo.MPSTLItems", "SubFName");
            DropColumn("dbo.MPSTLItems", "SubFNumber");
            CreateIndex("dbo.MPSYPItems", "SubItem_ID");
            CreateIndex("dbo.t_ICItem", "FSource");
            CreateIndex("dbo.MPSTLItems", "SubItem_ID");
            AddForeignKey("dbo.MPSYPItems", "SubItem_ID", "dbo.t_ICItem", "ID");
            AddForeignKey("dbo.MPSTLItems", "SubItem_ID", "dbo.t_ICItem", "ID");
            AddForeignKey("dbo.t_ICItem", "FSource", "dbo.t_Department", "FItemID");
        }
    }
}
