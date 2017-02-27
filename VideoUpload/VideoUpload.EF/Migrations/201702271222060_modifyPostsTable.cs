namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyPostsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "EditedBy", c => c.String());
            AddColumn("dbo.Posts", "DateEdited", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "DateEdited");
            DropColumn("dbo.Posts", "EditedBy");
        }
    }
}
