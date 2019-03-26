namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToStringPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "perpetual", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Contacts", "phoneNumber", c => c.String());
            AlterColumn("dbo.Contacts", "altPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "altPhoneNumber", c => c.Int());
            AlterColumn("dbo.Contacts", "phoneNumber", c => c.Int());
            DropColumn("dbo.Contacts", "perpetual");
        }
    }
}
