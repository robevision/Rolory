namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        gender = c.String(),
                        relationship = c.String(),
                        category = c.String(),
                        birthDate = c.DateTime(),
                        deathDate = c.DateTime(),
                        anniversary = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Descriptions");
        }
    }
}
