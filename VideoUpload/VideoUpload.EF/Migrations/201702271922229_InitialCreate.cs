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
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Owner = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                    })
                .PrimaryKey(t => t.PostID);
            
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
            DropForeignKey("dbo.Attachments", "PostID", "dbo.Posts");
            DropIndex("dbo.Attachments", new[] { "PostID" });
            DropTable("dbo.Histories");
            DropTable("dbo.Posts");
            DropTable("dbo.Attachments");
        }
    }
}
