namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserClaimUserIdChangedToOptional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserClaims", "UserID", "dbo.Users");
            DropIndex("dbo.UserClaims", new[] { "UserID" });
            AlterColumn("dbo.UserClaims", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserClaims", "UserID");
            AddForeignKey("dbo.UserClaims", "UserID", "dbo.Users", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaims", "UserID", "dbo.Users");
            DropIndex("dbo.UserClaims", new[] { "UserID" });
            AlterColumn("dbo.UserClaims", "UserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.UserClaims", "UserID");
            AddForeignKey("dbo.UserClaims", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
