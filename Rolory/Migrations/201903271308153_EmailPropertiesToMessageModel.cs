namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailPropertiesToMessageModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "ToEmail", c => c.String());
            AddColumn("dbo.Messages", "EmailCC", c => c.String());
            AddColumn("dbo.Messages", "EmailBCC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "EmailBCC");
            DropColumn("dbo.Messages", "EmailCC");
            DropColumn("dbo.Messages", "ToEmail");
        }
    }
}
