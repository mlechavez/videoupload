namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyHistories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attachments", "PostID", "dbo.Posts");
            DropIndex("dbo.Attachments", new[] { "PostID" });
            AddColumn("dbo.Histories", "Name", c => c.String());
            AddColumn("dbo.Histories", "UserID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Histories", "PostID", c => c.Int(nullable: false));
            AlterColumn("dbo.Attachments", "PostID", c => c.Int(nullable: false));
            CreateIndex("dbo.Attachments", "PostID");
            CreateIndex("dbo.Histories", "UserID");
            CreateIndex("dbo.Histories", "PostID");
            AddForeignKey("dbo.Histories", "UserID", "dbo.Users", "UserID", cascadeDelete: false);
            AddForeignKey("dbo.Histories", "PostID", "dbo.Posts", "PostID", cascadeDelete: false);
            AddForeignKey("dbo.Attachments", "PostID", "dbo.Posts", "PostID", cascadeDelete: false);
            DropColumn("dbo.Histories", "Sender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Histories", "Sender", c => c.String());
            DropForeignKey("dbo.Attachments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Histories", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Histories", "UserID", "dbo.Users");
            DropIndex("dbo.Histories", new[] { "PostID" });
            DropIndex("dbo.Histories", new[] { "UserID" });
            DropIndex("dbo.Attachments", new[] { "PostID" });
            AlterColumn("dbo.Attachments", "PostID", c => c.Int());
            DropColumn("dbo.Histories", "PostID");
            DropColumn("dbo.Histories", "UserID");
            DropColumn("dbo.Histories", "Name");
            CreateIndex("dbo.Attachments", "PostID");
            AddForeignKey("dbo.Attachments", "PostID", "dbo.Posts", "PostID");
        }
    }
}
