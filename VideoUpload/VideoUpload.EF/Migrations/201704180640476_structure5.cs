namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class structure5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HealthCheckDetails", "HealthCheck_HcCode", "dbo.HealthChecks");
            DropIndex("dbo.HealthCheckDetails", new[] { "HealthCheck_HcCode" });
            DropColumn("dbo.HealthCheckDetails", "HcCode");
            RenameColumn(table: "dbo.HealthCheckDetails", name: "HealthCheck_HcCode", newName: "HcCode");
            AlterColumn("dbo.HealthCheckDetails", "HcCode", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.HealthCheckDetails", "JobcardNo", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.HealthCheckDetails", "HcCode");
            CreateIndex("dbo.HealthCheckDetails", "JobcardNo");
            AddForeignKey("dbo.HealthCheckDetails", "JobcardNo", "dbo.Jobcards", "JobcardNo", cascadeDelete: true);
            AddForeignKey("dbo.HealthCheckDetails", "HcCode", "dbo.HealthChecks", "HcCode", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HealthCheckDetails", "HcCode", "dbo.HealthChecks");
            DropForeignKey("dbo.HealthCheckDetails", "JobcardNo", "dbo.Jobcards");
            DropIndex("dbo.HealthCheckDetails", new[] { "JobcardNo" });
            DropIndex("dbo.HealthCheckDetails", new[] { "HcCode" });
            AlterColumn("dbo.HealthCheckDetails", "JobcardNo", c => c.String());
            AlterColumn("dbo.HealthCheckDetails", "HcCode", c => c.String());
            RenameColumn(table: "dbo.HealthCheckDetails", name: "HcCode", newName: "HealthCheck_HcCode");
            AddColumn("dbo.HealthCheckDetails", "HcCode", c => c.String());
            CreateIndex("dbo.HealthCheckDetails", "HealthCheck_HcCode");
            AddForeignKey("dbo.HealthCheckDetails", "HealthCheck_HcCode", "dbo.HealthChecks", "HcCode");
        }
    }
}
