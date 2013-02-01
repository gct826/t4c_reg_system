namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PaymentEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentEntries", "PmtTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentEntries", "PmtStatusID", c => c.Int(nullable: false));
            AddForeignKey("dbo.PaymentEntries", "PmtStatusID", "dbo.PmtStatus", "PmtStatusID", cascadeDelete: true);
            AddForeignKey("dbo.PaymentEntries", "PmtTypeID", "dbo.PmtTypes", "PmtTypeID", cascadeDelete: true);
            CreateIndex("dbo.PaymentEntries", "PmtStatusID");
            CreateIndex("dbo.PaymentEntries", "PmtTypeID");
            DropColumn("dbo.PaymentEntries", "PaymentType");
            DropColumn("dbo.PaymentEntries", "PaymentStat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentEntries", "PaymentStat", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentEntries", "PaymentType", c => c.String());
            DropIndex("dbo.PaymentEntries", new[] { "PmtTypeID" });
            DropIndex("dbo.PaymentEntries", new[] { "PmtStatusID" });
            DropForeignKey("dbo.PaymentEntries", "PmtTypeID", "dbo.PmtTypes");
            DropForeignKey("dbo.PaymentEntries", "PmtStatusID", "dbo.PmtStatus");
            DropColumn("dbo.PaymentEntries", "PmtStatusID");
            DropColumn("dbo.PaymentEntries", "PmtTypeID");
        }
    }
}
