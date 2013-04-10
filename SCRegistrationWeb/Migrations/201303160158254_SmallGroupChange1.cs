namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallGroupChange1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SmallGroups", "SmallGroupID", c => c.Int(nullable: false, identity: true));
            //DropPrimaryKey("dbo.SmallGroups", new[] { "SmallGropuID" });
            //AddPrimaryKey("dbo.SmallGroups", "SmallGroupID");
            //DropColumn("dbo.SmallGroups", "SmallGropuID");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.SmallGroups", "SmallGropuID", c => c.Int(nullable: false, identity: true));
            //DropPrimaryKey("dbo.SmallGroups", new[] { "SmallGroupID" });
            //AddPrimaryKey("dbo.SmallGroups", "SmallGropuID");
            //DropColumn("dbo.SmallGroups", "SmallGroupID");
        }
    }
}
