namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeasonIntSharedActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SharedActivities", "Season", c => c.Int());
            AddColumn("dbo.SharedActivities", "CoolDownTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SharedActivities", "CoolDownTime");
            DropColumn("dbo.SharedActivities", "Season");
        }
    }
}
