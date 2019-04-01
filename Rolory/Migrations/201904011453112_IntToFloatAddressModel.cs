namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToFloatAddressModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "Latitude", c => c.Single());
            AlterColumn("dbo.Addresses", "Longitude", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "Longitude", c => c.Int());
            AlterColumn("dbo.Addresses", "Latitude", c => c.Int());
        }
    }
}
