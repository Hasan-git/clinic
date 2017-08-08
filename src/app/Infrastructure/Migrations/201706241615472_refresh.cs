namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Assistant", "Email2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assistant", "Email2", c => c.String());
        }
    }
}
