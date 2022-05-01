namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProductAndOrderDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 300));
            AddColumn("dbo.Products", "Alias", c => c.String(nullable: false, maxLength: 300, defaultValue: ""));
            AddColumn("dbo.Products", "ThumbImage", c => c.String(nullable: false, maxLength: 300, defaultValue: "~/Images/noimage.jpg"));
            AddColumn("dbo.OrderDetails", "Notes", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Notes");
            DropColumn("dbo.Products", "ThumbImage");
            DropColumn("dbo.Products", "Alias");
            DropColumn("dbo.Products", "Name");
        }
    }
}
