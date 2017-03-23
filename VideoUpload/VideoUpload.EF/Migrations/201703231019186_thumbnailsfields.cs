namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thumbnailsfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "ThumbnailFileName", c => c.String());
            AddColumn("dbo.Attachments", "ThumbnailUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "ThumbnailUrl");
            DropColumn("dbo.Attachments", "ThumbnailFileName");
        }
    }
}
