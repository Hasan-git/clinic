namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointment", "Reason", c => c.String());
            AddColumn("dbo.Appointment", "Address", c => c.String());
            AddColumn("dbo.Appointment", "LastVisit", c => c.String());
            AddColumn("dbo.Appointment", "LastVisitType", c => c.String());
            AddColumn("dbo.Appointment", "EventStatus", c => c.String());
            AddColumn("dbo.Appointment", "LastVisitId", c => c.Guid());
            DropColumn("dbo.Patient", "TestField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patient", "TestField", c => c.String());
            DropColumn("dbo.Appointment", "LastVisitId");
            DropColumn("dbo.Appointment", "EventStatus");
            DropColumn("dbo.Appointment", "LastVisitType");
            DropColumn("dbo.Appointment", "LastVisit");
            DropColumn("dbo.Appointment", "Address");
            DropColumn("dbo.Appointment", "Reason");
        }
    }
}
