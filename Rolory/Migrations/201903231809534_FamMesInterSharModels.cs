namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FamMesInterSharModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FamilyMembers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        relation = c.String(),
                        descriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Descriptions", t => t.descriptionId, cascadeDelete: true)
                .Index(t => t.descriptionId);
            
            CreateTable(
                "dbo.Interactions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        moment = c.DateTime(nullable: false),
                        messageId = c.Int(),
                        contactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Contacts", t => t.contactId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.messageId)
                .Index(t => t.messageId)
                .Index(t => t.contactId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        subject = c.String(),
                        body = c.String(),
                        postmark = c.DateTime(nullable: false),
                        isActive = c.Boolean(),
                        networkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Networkers", t => t.networkerId, cascadeDelete: true)
                .Index(t => t.networkerId);
            
            CreateTable(
                "dbo.SharedActivities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        activity = c.String(),
                        bond = c.String(),
                        descriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Descriptions", t => t.descriptionId, cascadeDelete: true)
                .Index(t => t.descriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedActivities", "descriptionId", "dbo.Descriptions");
            DropForeignKey("dbo.Interactions", "messageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "networkerId", "dbo.Networkers");
            DropForeignKey("dbo.Interactions", "contactId", "dbo.Contacts");
            DropForeignKey("dbo.FamilyMembers", "descriptionId", "dbo.Descriptions");
            DropIndex("dbo.SharedActivities", new[] { "descriptionId" });
            DropIndex("dbo.Messages", new[] { "networkerId" });
            DropIndex("dbo.Interactions", new[] { "contactId" });
            DropIndex("dbo.Interactions", new[] { "messageId" });
            DropIndex("dbo.FamilyMembers", new[] { "descriptionId" });
            DropTable("dbo.SharedActivities");
            DropTable("dbo.Messages");
            DropTable("dbo.Interactions");
            DropTable("dbo.FamilyMembers");
        }
    }
}
