namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPSTLItems", "LuCode", c => c.String());
            AddColumn("dbo.MPSTLItems", "PCH", c => c.String());
            AddColumn("dbo.MPSTLItems", "LuCode2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPSTLItems", "LuCode2");
            DropColumn("dbo.MPSTLItems", "PCH");
            DropColumn("dbo.MPSTLItems", "LuCode");
        }
    }
}
