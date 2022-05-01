namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRegister : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserProfiles", name: "AcountID", newName: "AccountID");
            RenameIndex(table: "dbo.UserProfiles", name: "IX_AcountID", newName: "IX_AccountID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserProfiles", name: "IX_AccountID", newName: "IX_AcountID");
            RenameColumn(table: "dbo.UserProfiles", name: "AccountID", newName: "AcountID");
        }
    }
}
