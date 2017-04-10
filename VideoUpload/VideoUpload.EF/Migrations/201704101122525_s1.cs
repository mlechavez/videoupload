namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Users", "BranchID", "dbo.Branches");
            DropIndex("dbo.Posts", new[] { "BranchID" });
            DropIndex("dbo.Jobcards", new[] { "BranchID" });
            DropIndex("dbo.Users", new[] { "BranchID" });
            AlterColumn("dbo.Posts", "BranchID", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobcards", "BranchID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "BranchID", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "BranchID");
            CreateIndex("dbo.Jobcards", "BranchID");
            CreateIndex("dbo.Users", "BranchID");
            AddForeignKey("dbo.Posts", "BranchID", "dbo.Branches", "BranchID", cascadeDelete: true);
            AddForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches", "BranchID", cascadeDelete: true);
            AddForeignKey("dbo.Users", "BranchID", "dbo.Branches", "BranchID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches");
            DropForeignKey("dbo.Posts", "BranchID", "dbo.Branches");
            DropIndex("dbo.Users", new[] { "BranchID" });
            DropIndex("dbo.Jobcards", new[] { "BranchID" });
            DropIndex("dbo.Posts", new[] { "BranchID" });
            AlterColumn("dbo.Users", "BranchID", c => c.Int());
            AlterColumn("dbo.Jobcards", "BranchID", c => c.Int());
            AlterColumn("dbo.Posts", "BranchID", c => c.Int());
            CreateIndex("dbo.Users", "BranchID");
            CreateIndex("dbo.Jobcards", "BranchID");
            CreateIndex("dbo.Posts", "BranchID");
            AddForeignKey("dbo.Users", "BranchID", "dbo.Branches", "BranchID");
            AddForeignKey("dbo.Jobcards", "BranchID", "dbo.Branches", "BranchID");
            AddForeignKey("dbo.Posts", "BranchID", "dbo.Branches", "BranchID");
        }
    }
}
