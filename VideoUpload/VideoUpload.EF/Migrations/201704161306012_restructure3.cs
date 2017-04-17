namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restructure3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthChecks", "IsGoodCondition", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthChecks", "IsSuggestedToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthChecks", "IsUrgentToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthChecks", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HealthChecks", "Comments");
            DropColumn("dbo.HealthChecks", "IsUrgentToReplace");
            DropColumn("dbo.HealthChecks", "IsSuggestedToReplace");
            DropColumn("dbo.HealthChecks", "IsGoodCondition");
        }
    }
}
