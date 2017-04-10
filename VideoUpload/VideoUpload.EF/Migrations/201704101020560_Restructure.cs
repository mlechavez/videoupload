namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Restructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchID = c.Int(nullable: false, identity: true),
                        BranchName = c.String(),
                    })
                .PrimaryKey(t => t.BranchID);
            
            AddColumn("dbo.Posts", "BranchID", c => c.Int());
            AddColumn("dbo.Users", "BranchID", c => c.Int());
            AddColumn("dbo.Jobcards", "BranchID", c => c.Int());
            CreateIndex("dbo.Posts", "BranchID");
            CreateIndex("dbo.Jobcards", "BranchID");
            CreateIndex("dbo.Users", "BranchID");
            AddForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches", "BranchID");
            AddForeignKey("dbo.Posts", "BranchID", "dbo.Branches", "BranchID");
            AddForeignKey("dbo.Users", "BranchID", "dbo.Branches", "BranchID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Posts", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches");
            DropIndex("dbo.Users", new[] { "BranchID" });
            DropIndex("dbo.Jobcards", new[] { "BranchID" });
            DropIndex("dbo.Posts", new[] { "BranchID" });
            DropColumn("dbo.Jobcards", "BranchID");
            DropColumn("dbo.Users", "BranchID");
            DropColumn("dbo.Posts", "BranchID");
            DropTable("dbo.Branches");
        }
    }
}
