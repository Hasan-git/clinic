namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillableAppoitment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointment", "Payment", c => c.String());
            AddColumn("dbo.Appointment", "IsPaid", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointment", "IsPaid");
            DropColumn("dbo.Appointment", "Payment");
        }
    }
}
