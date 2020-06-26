namespace ProduceFeeder.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MPSTLItems", "FCancel", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MPSTLItems", "FCancel");
        }
    }
}
