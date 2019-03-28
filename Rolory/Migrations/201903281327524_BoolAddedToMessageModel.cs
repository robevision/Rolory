namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoolAddedToMessageModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "IsInteraction", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "IsInteraction");
        }
    }
}
