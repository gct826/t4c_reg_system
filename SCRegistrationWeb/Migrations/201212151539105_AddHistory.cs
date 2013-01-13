namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventHistories",
                c => new
                    {
                        HistoryID = c.Int(nullable: false, identity: true),
                        HistoryDate = c.DateTime(nullable: false),
                        RegHistID = c.Int(nullable: false),
                        HistoryEvent = c.String(),
                        EventHistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventHistories");
        }
    }
}
