namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RoomAssignment_DeleteFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomAssignments", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomAssignments", "IsDeleted");
        }
    }
}
