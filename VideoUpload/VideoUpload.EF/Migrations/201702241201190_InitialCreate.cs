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
                    })
                .PrimaryKey(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "PostID", "dbo.Posts");
            DropIndex("dbo.Attachments", new[] { "PostID" });
            DropTable("dbo.Posts");
            DropTable("dbo.Attachments");
        }
    }
}
