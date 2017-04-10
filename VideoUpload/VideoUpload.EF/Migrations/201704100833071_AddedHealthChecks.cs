namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHealthChecks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerCode = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        FirstName = c.String(),
                        SecondName = c.String(),
                        ThirdName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(),
                        CompanyName = c.String(),
                        Position = c.String(),
                        Department = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        SendEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerCode);
            
            CreateTable(
                "dbo.HealthCheckDetails",
                c => new
                    {
                        HealCheckDetailsID = c.Int(nullable: false, identity: true),
                        HcCode = c.String(),
                        JobcardNo = c.String(),
                        IsGoodCondition = c.Boolean(nullable: false),
                        IsSuggestedToReplace = c.Boolean(nullable: false),
                        IsUrgentToReplace = c.Boolean(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.HealCheckDetailsID);
            
            CreateTable(
                "dbo.HealthChecks",
                c => new
                    {
                        HcCode = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        HcGroup = c.String(),
                    })
                .PrimaryKey(t => t.HcCode);
            
            CreateTable(
                "dbo.Jobcards",
                c => new
                    {
                        JobcardNo = c.String(nullable: false, maxLength: 128),
                        CustomerName = c.String(),
                        ChassisNo = c.String(),
                        PlateNo = c.String(),
                        Mileage = c.String(),
                    })
                .PrimaryKey(t => t.JobcardNo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jobcards");
            DropTable("dbo.HealthChecks");
            DropTable("dbo.HealthCheckDetails");
            DropTable("dbo.Customers");
        }
    }
}
