namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lol : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointment", "Reason");
            DropColumn("dbo.Appointment", "Address");
            DropColumn("dbo.Appointment", "LastVisit");
            DropColumn("dbo.Appointment", "EventStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointment", "EventStatus", c => c.String());
            AddColumn("dbo.Appointment", "LastVisit", c => c.String());
            AddColumn("dbo.Appointment", "Address", c => c.String());
            AddColumn("dbo.Appointment", "Reason", c => c.String());
        }
    }
}
