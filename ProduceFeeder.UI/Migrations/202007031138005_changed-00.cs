namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed00 : DbMigration
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
                        LuCode = c.String(),
                        PCH = c.String(),
                        LuCode2 = c.String(),
                        FSourceBillNo = c.String(),
                        FSourceInnerID = c.Int(nullable: false),
                        RunningStatus = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        FeedingDate = c.DateTime(),
                        OutFeedingDate = c.DateTime(),
                        RclLine = c.String(),
                        FeedingType = c.Int(nullable: false),
                        Remark = c.String(),
                        SubFNumber = c.String(),
                        SubFName = c.String(),
                        SubFModel = c.String(),
                        SubFItemId = c.Int(nullable: false),
                        BillNo = c.String(),
                        QJId = c.Int(nullable: false),
                        OnePlanQTY = c.Int(nullable: false),
                        PlanStamp = c.String(maxLength: 30),
                        Qty = c.Int(nullable: false),
                        DXQ = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Feeder = c.String(maxLength: 30),
                        CPItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.K3CPItemBase", t => t.CPItem_ID)
                .Index(t => t.CPItem_ID);
            
            CreateTable(
                "dbo.K3CPItemBase",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        K3ItemID = c.Int(nullable: false),
                        K3FNumber = c.String(),
                        K3FModel = c.String(),
                        K3FName = c.String(),
                        FCustObjId = c.Int(nullable: false),
                        FRoutingId = c.Int(nullable: false),
                        DepFName = c.String(),
                        DepId = c.Int(nullable: false),
                        BOMId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        SubFNumber = c.String(),
                        SubFName = c.String(),
                        SubFModel = c.String(),
                        SubFItemId = c.Int(nullable: false),
                        BillNo = c.String(),
                        QJId = c.Int(nullable: false),
                        OnePlanQTY = c.Int(nullable: false),
                        PlanStamp = c.String(maxLength: 30),
                        Qty = c.Int(nullable: false),
                        DXQ = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Feeder = c.String(maxLength: 30),
                        CPItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.K3CPItemBase", t => t.CPItem_ID)
                .Index(t => t.CPItem_ID);
            
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
            DropForeignKey("dbo.MPSYPItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropForeignKey("dbo.MPSPlanItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropForeignKey("dbo.MPSTLItems", "CPItem_ID", "dbo.K3CPItemBase");
            DropIndex("dbo.RclRunCalendars", new[] { "RclID" });
            DropIndex("dbo.MPSYPItems", new[] { "CPItem_ID" });
            DropIndex("dbo.MPSPlanItems", new[] { "CPItem_ID" });
            DropIndex("dbo.MPSTLItems", new[] { "CPItem_ID" });
            DropTable("dbo.RclRunCalendars");
            DropTable("dbo.wjj_RclMachine");
            DropTable("dbo.MPSYPItems");
            DropTable("dbo.MPSPlanItems");
            DropTable("dbo.K3CPItemBase");
            DropTable("dbo.MPSTLItems");
        }
    }
}
