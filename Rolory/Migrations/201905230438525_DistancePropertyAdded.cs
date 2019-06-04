namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DistancePropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Distance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Distance");
        }
    }
}
