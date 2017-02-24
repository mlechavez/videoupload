namespace VideoUpload.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedHistoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        Recipient = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        Sender = c.String(),
                    })
                .PrimaryKey(t => t.HistoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Histories");
        }
    }
}
