namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentEntry1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentEntries", "RegistrationEntries_RegistrationID", c => c.Int());
            AddForeignKey("dbo.PaymentEntries", "RegistrationEntries_RegistrationID", "dbo.RegistrationEntries", "RegistrationID");
            CreateIndex("dbo.PaymentEntries", "RegistrationEntries_RegistrationID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PaymentEntries", new[] { "RegistrationEntries_RegistrationID" });
            DropForeignKey("dbo.PaymentEntries", "RegistrationEntries_RegistrationID", "dbo.RegistrationEntries");
            DropColumn("dbo.PaymentEntries", "RegistrationEntries_RegistrationID");
        }
    }
}
