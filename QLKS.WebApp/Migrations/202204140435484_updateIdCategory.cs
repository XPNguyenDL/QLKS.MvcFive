namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIdCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ParentLevel", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ParentLevel");
        }
    }
}
