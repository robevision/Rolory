namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReBuildingDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressType = c.String(),
                        StreetAddress = c.String(),
                        Unit = c.String(),
                        Locality = c.String(),
                        Region = c.String(),
                        ZipCode = c.Int(),
                        CountryName = c.String(),
                        Latitude = c.Single(),
                        Longitude = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Email = c.String(),
                        Prefix = c.String(),
                        GivenName = c.String(),
                        FamilyName = c.String(),
                        PhoneType = c.String(),
                        PhoneNumber = c.String(),
                        Organization = c.String(),
                        WorkTitle = c.String(),
                        AltPhoneNumberType = c.String(),
                        AltPhoneNumber = c.String(),
                        LastUpdated = c.DateTime(),
                        InContact = c.Boolean(nullable: false),
                        InContactCountDown = c.DateTime(),
                        Perpetual = c.Boolean(nullable: false),
                        CoolDown = c.Boolean(nullable: false),
                        CoolDownTime = c.DateTime(),
                        Reminder = c.DateTime(),
                        AddressId = c.Int(),
                        AltAddressId = c.Int(),
                        DescriptionId = c.Int(nullable: false),
                        NetworkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Addresses", t => t.AltAddressId)
                .ForeignKey("dbo.Descriptions", t => t.DescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.Networkers", t => t.NetworkerId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.AltAddressId)
                .Index(t => t.DescriptionId)
                .Index(t => t.NetworkerId);
            
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gender = c.String(),
                        Relationship = c.String(),
                        Category = c.String(),
                        BirthDate = c.DateTime(),
                        DeathDate = c.DateTime(),
                        Anniversary = c.DateTime(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Networkers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ReceiveEmails = c.Boolean(nullable: false),
                        EmailQuota = c.Boolean(nullable: false),
                        EmailFrequency = c.Int(nullable: false),
                        UserActivities = c.String(),
                        Occupation = c.String(),
                        WorkTitle = c.String(),
                        RunningTally = c.Int(nullable: false),
                        Goal = c.Int(),
                        GoalActive = c.Boolean(nullable: false),
                        GoalStatus = c.Boolean(nullable: false),
                        GoalCoolDown = c.DateTime(nullable: false),
                        Availability = c.String(),
                        UserId = c.String(maxLength: 128),
                        AddressId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserRole = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.FamilyMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Relation = c.String(),
                        DescriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descriptions", t => t.DescriptionId, cascadeDelete: true)
                .Index(t => t.DescriptionId);
            
            CreateTable(
                "dbo.Interactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Moment = c.DateTime(nullable: false),
                        MessageId = c.Int(),
                        ContactId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.MessageId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        Postmark = c.DateTime(nullable: false),
                        ToEmail = c.String(),
                        EmailCC = c.String(),
                        EmailBCC = c.String(),
                        IsEmail = c.Boolean(nullable: false),
                        IsActive = c.Boolean(),
                        IsInteraction = c.Boolean(nullable: false),
                        NetworkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Networkers", t => t.NetworkerId, cascadeDelete: true)
                .Index(t => t.NetworkerId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SharedActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Activity = c.String(),
                        Bond = c.String(),
                        Season = c.Int(),
                        CoolDownTime = c.DateTime(),
                        DescriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descriptions", t => t.DescriptionId, cascadeDelete: true)
                .Index(t => t.DescriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SharedActivities", "DescriptionId", "dbo.Descriptions");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Interactions", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "NetworkerId", "dbo.Networkers");
            DropForeignKey("dbo.Interactions", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.FamilyMembers", "DescriptionId", "dbo.Descriptions");
            DropForeignKey("dbo.Contacts", "NetworkerId", "dbo.Networkers");
            DropForeignKey("dbo.Networkers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Networkers", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Contacts", "DescriptionId", "dbo.Descriptions");
            DropForeignKey("dbo.Contacts", "AltAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Contacts", "AddressId", "dbo.Addresses");
            DropIndex("dbo.SharedActivities", new[] { "DescriptionId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Messages", new[] { "NetworkerId" });
            DropIndex("dbo.Interactions", new[] { "ContactId" });
            DropIndex("dbo.Interactions", new[] { "MessageId" });
            DropIndex("dbo.FamilyMembers", new[] { "DescriptionId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Networkers", new[] { "AddressId" });
            DropIndex("dbo.Networkers", new[] { "UserId" });
            DropIndex("dbo.Contacts", new[] { "NetworkerId" });
            DropIndex("dbo.Contacts", new[] { "DescriptionId" });
            DropIndex("dbo.Contacts", new[] { "AltAddressId" });
            DropIndex("dbo.Contacts", new[] { "AddressId" });
            DropTable("dbo.SharedActivities");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.Interactions");
            DropTable("dbo.FamilyMembers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Networkers");
            DropTable("dbo.Descriptions");
            DropTable("dbo.Contacts");
            DropTable("dbo.Addresses");
        }
    }
}
