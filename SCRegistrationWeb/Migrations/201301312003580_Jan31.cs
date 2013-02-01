namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jan31 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentEntries", "RegID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentEntries", "RegID", c => c.String());
        }
    }
}
