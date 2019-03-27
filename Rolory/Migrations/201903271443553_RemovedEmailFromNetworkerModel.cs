namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedEmailFromNetworkerModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Networkers", "EmailAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Networkers", "EmailAddress", c => c.String());
        }
    }
}
