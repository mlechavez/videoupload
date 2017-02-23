namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedVideoFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "VideoFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "VideoFileName");
        }
    }
}
