namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNetworkerOccupationProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Networkers", "Occupation", c => c.String());
            AddColumn("dbo.Networkers", "WorkTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Networkers", "WorkTitle");
            DropColumn("dbo.Networkers", "Occupation");
        }
    }
}
