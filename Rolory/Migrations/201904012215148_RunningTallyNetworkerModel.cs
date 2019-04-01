namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RunningTallyNetworkerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "RunningTally", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "RunningTally");
        }
    }
}
