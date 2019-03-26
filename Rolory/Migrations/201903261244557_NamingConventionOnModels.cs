namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamingConventionOnModels : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Contacts", new[] { "addressId" });
            DropIndex("dbo.Contacts", new[] { "altAddressId" });
            DropIndex("dbo.Contacts", new[] { "descriptionId" });
            DropIndex("dbo.Contacts", new[] { "networkerId" });
            DropIndex("dbo.FamilyMembers", new[] { "descriptionId" });
            DropIndex("dbo.Interactions", new[] { "messageId" });
            DropIndex("dbo.Interactions", new[] { "contactId" });
            DropIndex("dbo.Messages", new[] { "networkerId" });
            DropIndex("dbo.SharedActivities", new[] { "descriptionId" });
            AddColumn("dbo.Descriptions", "Notes", c => c.String());
            AddColumn("dbo.Networkers", "UserActivities", c => c.String());
            CreateIndex("dbo.Contacts", "AddressId");
            CreateIndex("dbo.Contacts", "AltAddressId");
            CreateIndex("dbo.Contacts", "DescriptionId");
            CreateIndex("dbo.Contacts", "NetworkerId");
            CreateIndex("dbo.FamilyMembers", "DescriptionId");
            CreateIndex("dbo.Interactions", "MessageId");
            CreateIndex("dbo.Interactions", "ContactId");
            CreateIndex("dbo.Messages", "NetworkerId");
            CreateIndex("dbo.SharedActivities", "DescriptionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SharedActivities", new[] { "DescriptionId" });
            DropIndex("dbo.Messages", new[] { "NetworkerId" });
            DropIndex("dbo.Interactions", new[] { "ContactId" });
            DropIndex("dbo.Interactions", new[] { "MessageId" });
            DropIndex("dbo.FamilyMembers", new[] { "DescriptionId" });
            DropIndex("dbo.Contacts", new[] { "NetworkerId" });
            DropIndex("dbo.Contacts", new[] { "DescriptionId" });
            DropIndex("dbo.Contacts", new[] { "AltAddressId" });
            DropIndex("dbo.Contacts", new[] { "AddressId" });
            DropColumn("dbo.Networkers", "UserActivities");
            DropColumn("dbo.Descriptions", "Notes");
            CreateIndex("dbo.SharedActivities", "descriptionId");
            CreateIndex("dbo.Messages", "networkerId");
            CreateIndex("dbo.Interactions", "contactId");
            CreateIndex("dbo.Interactions", "messageId");
            CreateIndex("dbo.FamilyMembers", "descriptionId");
            CreateIndex("dbo.Contacts", "networkerId");
            CreateIndex("dbo.Contacts", "descriptionId");
            CreateIndex("dbo.Contacts", "altAddressId");
            CreateIndex("dbo.Contacts", "addressId");
        }
    }
}
