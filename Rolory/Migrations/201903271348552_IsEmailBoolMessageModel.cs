namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsEmailBoolMessageModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "IsEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "IsEmail");
        }
    }
}
