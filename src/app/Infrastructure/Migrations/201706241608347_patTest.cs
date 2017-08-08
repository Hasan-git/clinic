namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assistant", "Email2", c => c.String());
            DropColumn("dbo.Patient", "Referrer2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patient", "Referrer2", c => c.String());
            DropColumn("dbo.Assistant", "Email2");
        }
    }
}
