namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        JobTitle = c.String(),
                        EmployeeNo = c.String(),
                        Email = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 128),
                        SecurityStamp = c.String(maxLength: 128),
                        EmailPass = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        ClaimID = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ClaimID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            AddColumn("dbo.Posts", "PlateNumber", c => c.String(nullable: false));
            AddColumn("dbo.Posts", "DateUploaded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posts", "UserID", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "IsApproved", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Posts", "UserID");
            AddForeignKey("dbo.Posts", "UserID", "dbo.Users", "UserID");
            DropColumn("dbo.Posts", "Title");
            DropColumn("dbo.Posts", "Owner");
            DropColumn("dbo.Posts", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posts", "Owner", c => c.String());
            AddColumn("dbo.Posts", "Title", c => c.String(nullable: false));
            DropForeignKey("dbo.UserClaims", "UserID", "dbo.Users");
            DropForeignKey("dbo.Posts", "UserID", "dbo.Users");
            DropIndex("dbo.UserClaims", new[] { "UserID" });
            DropIndex("dbo.Posts", new[] { "UserID" });
            DropColumn("dbo.Posts", "IsApproved");
            DropColumn("dbo.Posts", "UserID");
            DropColumn("dbo.Posts", "DateUploaded");
            DropColumn("dbo.Posts", "PlateNumber");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
        }
    }
}
