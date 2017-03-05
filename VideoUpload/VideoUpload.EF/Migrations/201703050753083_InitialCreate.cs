namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        PostAttachmentID = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(),
                        MIMEType = c.String(),
                        FileSize = c.Int(nullable: false),
                        FileUrl = c.String(),
                        PostID = c.Int(),
                        AttachmentNo = c.String(),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.PostAttachmentID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        PlateNumber = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateUploaded = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        UserID = c.String(maxLength: 128),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
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
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        Recipient = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        Sender = c.String(),
                    })
                .PrimaryKey(t => t.HistoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaims", "UserID", "dbo.Users");
            DropForeignKey("dbo.Posts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Attachments", "PostID", "dbo.Posts");
            DropIndex("dbo.UserClaims", new[] { "UserID" });
            DropIndex("dbo.Posts", new[] { "UserID" });
            DropIndex("dbo.Attachments", new[] { "PostID" });
            DropTable("dbo.Histories");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Posts");
            DropTable("dbo.Attachments");
        }
    }
}
