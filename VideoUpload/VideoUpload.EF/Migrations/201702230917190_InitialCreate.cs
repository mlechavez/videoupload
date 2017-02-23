namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        VideoID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VideoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Videos");
        }
    }
}
