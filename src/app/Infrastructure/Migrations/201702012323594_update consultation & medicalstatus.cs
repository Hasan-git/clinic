namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateconsultationmedicalstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalStatus", "PastMedicalHistory", c => c.String());
            DropColumn("dbo.Consultation", "PastHistory");
            DropColumn("dbo.MedicalStatus", "Diseases");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicalStatus", "Diseases", c => c.String());
            AddColumn("dbo.Consultation", "PastHistory", c => c.String());
            DropColumn("dbo.MedicalStatus", "PastMedicalHistory");
        }
    }
}
