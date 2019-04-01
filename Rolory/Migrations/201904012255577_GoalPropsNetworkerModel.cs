namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalPropsNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "GoalActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Networkers", "GoalStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "GoalStatus");
            DropColumn("dbo.Networkers", "GoalActive");
        }
    }
}
