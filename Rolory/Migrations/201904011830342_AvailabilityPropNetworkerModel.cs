namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvailabilityPropNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "Availability", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "Availability");
        }
    }
}
