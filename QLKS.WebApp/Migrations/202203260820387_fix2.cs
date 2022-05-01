namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Actived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Suppliers", "Acctived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suppliers", "Acctived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Suppliers", "Actived");
        }
    }
}
