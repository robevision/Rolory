namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NetworkerModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Networkers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Networkers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Networkers", new[] { "UserId" });
            DropTable("dbo.Networkers");
        }
    }
}
