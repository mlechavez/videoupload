namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevisedDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attachments", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Posts", "DateUploaded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Posts", "DateEdited", c => c.DateTime());
            AlterColumn("dbo.Posts", "DateApproved", c => c.DateTime());
            AlterColumn("dbo.Histories", "DateSent", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Histories", "DateSent", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Posts", "DateApproved", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Posts", "DateEdited", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Posts", "DateUploaded", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Attachments", "DateCreated", c => c.DateTimeOffset(precision: 7));
        }
    }
}
