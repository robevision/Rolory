namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressFKtoNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Latitude", c => c.Int());
            AddColumn("dbo.Addresses", "Longitude", c => c.Int());
            AddColumn("dbo.Networkers", "Goal", c => c.Int());
            AddColumn("dbo.Networkers", "AddressId", c => c.Int());
            CreateIndex("dbo.Networkers", "AddressId");
            AddForeignKey("dbo.Networkers", "AddressId", "dbo.Addresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Networkers", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Networkers", new[] { "AddressId" });
            DropColumn("dbo.Networkers", "AddressId");
            DropColumn("dbo.Networkers", "Goal");
            DropColumn("dbo.Addresses", "Longitude");
            DropColumn("dbo.Addresses", "Latitude");
        }
    }
}
