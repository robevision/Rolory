namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalCoolDownPropNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "GoalCoolDown", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "GoalCoolDown");
        }
    }
}
