namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SendingVideoAlertFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "HasPlayedVideo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "DatePlayedVideo", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "DatePlayedVideo");
            DropColumn("dbo.Posts", "HasPlayedVideo");
        }
    }
}
