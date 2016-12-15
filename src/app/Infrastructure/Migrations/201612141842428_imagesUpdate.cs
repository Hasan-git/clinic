namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagesUpdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Images", new[] { "ConsultaionId" });
            DropIndex("dbo.Images", new[] { "FollowUpId" });
            RenameColumn(table: "dbo.Images", name: "ConsultaionId", newName: "Consultation_Id");
            RenameColumn(table: "dbo.Images", name: "FollowUpId", newName: "FollowUp_Id");
            AlterColumn("dbo.Images", "Consultation_Id", c => c.Guid());
            AlterColumn("dbo.Images", "FollowUp_Id", c => c.Guid());
            CreateIndex("dbo.Images", "FollowUp_Id");
            CreateIndex("dbo.Images", "Consultation_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Images", new[] { "Consultation_Id" });
            DropIndex("dbo.Images", new[] { "FollowUp_Id" });
            AlterColumn("dbo.Images", "FollowUp_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Images", "Consultation_Id", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Images", name: "FollowUp_Id", newName: "FollowUpId");
            RenameColumn(table: "dbo.Images", name: "Consultation_Id", newName: "ConsultaionId");
            CreateIndex("dbo.Images", "FollowUpId");
            CreateIndex("dbo.Images", "ConsultaionId");
        }
    }
}
