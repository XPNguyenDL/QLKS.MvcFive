namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactTile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "ContactTitle", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "ContactTitle");
        }
    }
}
