namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimePropsInContactModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "InContactCountDown", c => c.DateTime());
            AddColumn("dbo.Contacts", "Reminder", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Reminder");
            DropColumn("dbo.Contacts", "InContactCountDown");
        }
    }
}
