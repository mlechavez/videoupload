namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restructure4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HealthCheckDetails", "Status", c => c.String());
            DropColumn("dbo.HealthCheckDetails", "IsGoodCondition");
            DropColumn("dbo.HealthCheckDetails", "IsSuggestedToReplace");
            DropColumn("dbo.HealthCheckDetails", "IsUrgentToReplace");
            DropColumn("dbo.HealthChecks", "IsGoodCondition");
            DropColumn("dbo.HealthChecks", "IsSuggestedToReplace");
            DropColumn("dbo.HealthChecks", "IsUrgentToReplace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HealthChecks", "IsUrgentToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthChecks", "IsSuggestedToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthChecks", "IsGoodCondition", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthCheckDetails", "IsUrgentToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthCheckDetails", "IsSuggestedToReplace", c => c.Boolean(nullable: false));
            AddColumn("dbo.HealthCheckDetails", "IsGoodCondition", c => c.Boolean(nullable: false));
            DropColumn("dbo.HealthCheckDetails", "Status");
        }
    }
}
