namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNetworkerProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "receiveEmails", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "receiveEmails");
        }
    }
}
