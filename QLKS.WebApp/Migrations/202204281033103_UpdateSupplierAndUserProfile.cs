namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSupplierAndUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Password", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Suppliers", "Alias", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Alias");
            DropColumn("dbo.UserProfiles", "Password");
        }
    }
}
