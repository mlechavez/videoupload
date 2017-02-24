namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAttachmentNoField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "AttachmentNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "AttachmentNo");
        }
    }
}
