namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assistant", "TestField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assistant", "TestField", c => c.String());
        }
    }
}
