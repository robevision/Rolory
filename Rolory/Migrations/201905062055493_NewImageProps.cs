namespace Rolory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewImageProps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "ImageTitle", c => c.String());
            AddColumn("dbo.Contacts", "ImagePath", c => c.String());
            DropColumn("dbo.Contacts", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Image", c => c.String());
            DropColumn("dbo.Contacts", "ImagePath");
            DropColumn("dbo.Contacts", "ImageTitle");
        }
    }
}
