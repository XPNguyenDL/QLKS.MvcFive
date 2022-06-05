namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Image", c => c.String());
            AddColumn("dbo.Suppliers", "ShortInfo", c => c.String());
            DropColumn("dbo.Suppliers", "IconPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suppliers", "IconPath", c => c.String());
            DropColumn("dbo.Suppliers", "ShortInfo");
            DropColumn("dbo.Suppliers", "Image");
        }
    }
}
