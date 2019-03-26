namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoolDownPropertiesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "CoolDown", c => c.Boolean(nullable: false));
            AddColumn("dbo.Contacts", "CoolDownTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "CoolDownTime");
            DropColumn("dbo.Contacts", "CoolDown");
        }
    }
}
