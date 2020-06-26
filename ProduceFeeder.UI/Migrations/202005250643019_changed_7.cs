namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MPSTLItems", "FCancel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MPSTLItems", "FCancel", c => c.Boolean(nullable: false));
        }
    }
}
