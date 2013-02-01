namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payment_Properties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
            "dbo.PmtStatus",
            c => new
            {
                PmtStatusID = c.Int(nullable: false, identity: true),
                Name = c.String(nullable: false, maxLength: 20),
            })
            .PrimaryKey(t => t.PmtStatusID);

            CreateTable(
            "dbo.PmtTypes",
            c => new
            {
                PmtTypeID = c.Int(nullable: false, identity: true),
                Name = c.String(nullable: false, maxLength: 20),
            })
            .PrimaryKey(t => t.PmtTypeID);
        }
        
        public override void Down()
        {
            DropTable("dbo.PmtStatus");
            DropTable("dbo.PmtTypes");
        }
    }
}
