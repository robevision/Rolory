namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailPropsNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "EmailQuota", c => c.Boolean(nullable: false));
            AddColumn("dbo.Networkers", "EmailFrequency", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "EmailFrequency");
            DropColumn("dbo.Networkers", "EmailQuota");
        }
    }
}
