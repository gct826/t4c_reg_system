namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentEntries", "RegID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentEntries", "RegID");
        }
    }
}
