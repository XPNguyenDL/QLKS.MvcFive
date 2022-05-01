namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateParentCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ParentId", c => c.Guid());
            CreateIndex("dbo.Categories", "ParentId");
            AddForeignKey("dbo.Categories", "ParentId", "dbo.Categories", "CategoryId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropColumn("dbo.Categories", "ParentId");
        }
    }
}
