namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNetworkerEmailProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "EmailAddress");
        }
    }
}
