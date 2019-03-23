namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactModelWithAddedAddressFK : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        image = c.String(),
                        email = c.String(),
                        prefix = c.String(),
                        givenName = c.String(),
                        familyName = c.String(),
                        phoneType = c.String(),
                        phoneNumber = c.Int(),
                        organization = c.String(),
                        workTitle = c.String(),
                        altPhoneNumberType = c.String(),
                        altPhoneNumber = c.Int(),
                        lastupdated = c.DateTime(),
                        inContact = c.Boolean(nullable: false),
                        addressId = c.Int(),
                        altAddressId = c.Int(),
                        descriptionId = c.Int(),
                        networkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Addresses", t => t.addressId)
                .ForeignKey("dbo.Addresses", t => t.altAddressId)
                .ForeignKey("dbo.Descriptions", t => t.descriptionId)
                .ForeignKey("dbo.Networkers", t => t.networkerId, cascadeDelete: true)
                .Index(t => t.addressId)
                .Index(t => t.altAddressId)
                .Index(t => t.descriptionId)
                .Index(t => t.networkerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "networkerId", "dbo.Networkers");
            DropForeignKey("dbo.Contacts", "descriptionId", "dbo.Descriptions");
            DropForeignKey("dbo.Contacts", "altAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Contacts", "addressId", "dbo.Addresses");
            DropIndex("dbo.Contacts", new[] { "networkerId" });
            DropIndex("dbo.Contacts", new[] { "descriptionId" });
            DropIndex("dbo.Contacts", new[] { "altAddressId" });
            DropIndex("dbo.Contacts", new[] { "addressId" });
            DropTable("dbo.Contacts");
        }
    }
}
