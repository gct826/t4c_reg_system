namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentEntries",
                c => new
                    {
                        PaymentID = c.Int(nullable: false, identity: true),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentType = c.String(),
                        PaymentAmt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStat = c.Int(nullable: false),
                        PaymentComment = c.String(),
                    })
                .PrimaryKey(t => t.PaymentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentEntries");
        }
    }
}
