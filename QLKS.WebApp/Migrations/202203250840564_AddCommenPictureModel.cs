namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommenPictureModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Guid(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 300),
                        Subject = c.String(nullable: false, maxLength: 300),
                        Content = c.String(nullable: false, maxLength: 4000),
                        PostedTime = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        ReplyContent = c.String(maxLength: 4000),
                        ReplyTime = c.DateTime(),
                        AccountId = c.String(maxLength: 128),
                        ProductId = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductHistories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        AccountId = c.String(maxLength: 128),
                        ActionTime = c.DateTime(nullable: false),
                        HistoryAction = c.Int(nullable: false),
                        OriginalProduct = c.String(storeType: "ntext"),
                        ModifiedProduct = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Guid(nullable: false),
                        Caption = c.String(),
                        Path = c.String(),
                        OrderNo = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ProductId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductHistories", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Pictures", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Comments", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductHistories", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Comments", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Pictures", new[] { "ProductId" });
            DropIndex("dbo.ProductHistories", new[] { "AccountId" });
            DropIndex("dbo.ProductHistories", new[] { "ProductId" });
            DropIndex("dbo.Comments", new[] { "ProductId" });
            DropIndex("dbo.Comments", new[] { "AccountId" });
            DropTable("dbo.Pictures");
            DropTable("dbo.ProductHistories");
            DropTable("dbo.Comments");
        }
    }
}
