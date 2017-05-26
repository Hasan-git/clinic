namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patient", "TestField", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patient", "TestField");
        }
    }
}
