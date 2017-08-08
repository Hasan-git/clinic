namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestingMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assistant", "TestField", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assistant", "TestField");
        }
    }
}
