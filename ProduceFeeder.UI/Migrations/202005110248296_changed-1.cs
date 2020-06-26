namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPSTLItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IconPath = c.String(maxLength: 30),
                        HandleStatus = c.Int(nullable: false),
                        XH = c.Int(nullable: false),
                        FSourceBillNo = c.String(),
                        FSourceInnerID = c.Int(nullable: false),
                        RunningStatus = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        FeedingDate = c.DateTime(),
                        OutFeedingDate = c.DateTime(),
                        RclLine = c.String(),
                        FeedingType = c.Int(nullable: false),
                        Remark = c.String(),
                        BillNo = c.String(),
                        QJId = c.Int(nullable: false),
                        OnePlanQTY = c.Int(nullable: false),
                        PlanStamp = c.String(maxLength: 30),
                        Qty = c.Int(nullable: false),
                        DXQ = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Feeder = c.String(maxLength: 30),
                        CPItem_ID = c.Int(),
                        SubItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.K3CPItemBase", t => t.CPItem_ID)
                .ForeignKey("dbo.t_ICItem", t => t.SubItem_ID)
                .Index(t => t.CPItem_ID)
                .Index(t => t.SubItem_ID);
            
            CreateTable(
                "dbo.K3CPItemBase",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        K3ItemID = c.Int(nullable: false),
                        K3FNumber = c.String(),
                        K3FModel = c.String(),
                        K3FName = c.String(),
                        BOMId = c.Int(nullable: false),
                        K3Dep_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.t_Department", t => t.K3Dep_Id)
                .Index(t => t.K3Dep_Id);
            
            CreateTable(
                "dbo.t_Department",
                c => new
                    {
                        FItemID = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                    })
                .PrimaryKey(t => t.FItemID);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.t_Department", t => t.FSource)
                .Index(t => t.FSource);
            
            CreateTable(
                "dbo.MPSPlanItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillNo = c.String(),
                        RunningStatus = c.Int(nullable: false),
                        Creator = c.String(),
                        PlanTotalQty = c.Int(nullable: false),
                        PlanCount = c.Int(nullable: false),
                        QJId = c.Int(nullable: false),
                        CPDep = c.String(),
                        PlanTLQty = c.Int(nullable: false),
                        ZJHQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxOutputQty = c.Int(nullable: false),
                        MinRawItemTL = c.Int(nullable: false),
                        CPItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.K3CPItemBase", t => t.CPItem_ID)
                .Index(t => t.CPItem_ID);
            
            CreateTable(
                "dbo.MPSYPItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RunningStatus = c.Int(nullable: false),
                        FeedingType = c.String(),
                        SourceBillNo = c.String(),
                        MaxUseableQty = c.Int(nullable: false),
                        MaxOutputQty = c.Int(nullable: false),
                        BillNo = c.String(),
                        QJId = c.Int(nullable: false),
                        OnePlanQTY = c.Int(nullable: false),
                        PlanStamp = c.String(maxLength: 30),
                        Qty = c.Int(nullable: false),
                        DXQ = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Feeder = c.String(maxLength: 30),
                        CPItem_ID = c.Int(),
                        SubItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.K3CPItemBase", t => t.CPItem_ID)
                .ForeignKey("dbo.t_ICItem", t => t.SubItem_ID)
                .Index(t => t.CPItem_ID)
                .Index(t => t.SubItem_ID);
            
            CreateTable(
                "dbo.wjj_RclMachine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        No = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RclRunCalendars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        StopDate = c.DateTime(nullable: false),
                        RclID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.wjj_RclMachine", t => t.RclID, cascadeDelete: true)
                .Index(t => t.RclID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RclRunCalendars", "RclID", "dbo.wjj_RclMachine");
            DropForeignKey("dbo.MPSYPItems", "SubItem_ID", "dbo.t_ICItem");
            DropForeignKey("dbo.MPSYPItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropForeignKey("dbo.MPSPlanItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropForeignKey("dbo.MPSTLItems", "SubItem_ID", "dbo.t_ICItem");
            DropForeignKey("dbo.t_ICItem", "FSource", "dbo.t_Department");
            DropForeignKey("dbo.MPSTLItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropForeignKey("dbo.K3CPItemBase", "K3Dep_Id", "dbo.t_Department");
            DropIndex("dbo.RclRunCalendars", new[] { "RclID" });
            DropIndex("dbo.MPSYPItems", new[] { "SubItem_ID" });
            DropIndex("dbo.MPSYPItems", new[] { "CPItem_ID" });
            DropIndex("dbo.MPSPlanItems", new[] { "CPItem_ID" });
            DropIndex("dbo.t_ICItem", new[] { "FSource" });
            DropIndex("dbo.K3CPItemBase", new[] { "K3Dep_Id" });
            DropIndex("dbo.MPSTLItems", new[] { "SubItem_ID" });
            DropIndex("dbo.MPSTLItems", new[] { "CPItem_ID" });
            DropTable("dbo.RclRunCalendars");
            DropTable("dbo.wjj_RclMachine");
            DropTable("dbo.MPSYPItems");
            DropTable("dbo.MPSPlanItems");
            DropTable("dbo.t_ICItem");
            DropTable("dbo.t_Department");
            DropTable("dbo.K3CPItemBase");
            DropTable("dbo.MPSTLItems");
        }
    }
}
