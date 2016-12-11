namespace Clinic.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Datetime = c.DateTime(),
                        Title = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        AllDay = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PatientName = c.String(),
                        ExistingPatient = c.Boolean(),
                        Mobile = c.String(),
                        Description = c.String(),
                        PatientId = c.Guid(),
                        DoctorId = c.Guid(nullable: false),
                        AssistantId = c.Guid(),
                        ClinicId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assistant", t => t.AssistantId)
                .ForeignKey("dbo.Clinic", t => t.ClinicId)
                .ForeignKey("dbo.Doctor", t => t.DoctorId)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.AssistantId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.Assistant",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        MiddelName = c.String(),
                        LastName = c.String(),
                        Mobile = c.Int(nullable: false),
                        Phone = c.Int(),
                        Email = c.String(),
                        AdditionalInformation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clinic",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Consultation",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Systolic = c.String(),
                        Diastolic = c.String(),
                        HeartRate = c.String(),
                        Temprature = c.String(),
                        Title = c.String(),
                        Subjective = c.String(),
                        Objective = c.String(),
                        Assessment = c.String(),
                        Plan = c.String(),
                        Cost = c.String(),
                        Paid = c.String(),
                        AdditionalInformation = c.String(),
                        PatientId = c.Guid(nullable: false),
                        DoctorId = c.Guid(nullable: false),
                        ClinicId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinic", t => t.ClinicId)
                .ForeignKey("dbo.Doctor", t => t.DoctorId)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.Doctor",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        MiddelName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Mobile = c.Int(nullable: false),
                        Phone = c.Int(),
                        Speciality = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        AdditionalInformation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        MiddelName = c.String(),
                        LastName = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        BloodType = c.String(),
                        Gender = c.String(),
                        Mobile = c.Int(nullable: false),
                        Phone = c.Int(),
                        Email = c.String(),
                        Address = c.String(),
                        AdditionalInformation = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        DoctorId = c.Guid(nullable: false),
                        ClinicId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinic", t => t.ClinicId)
                .ForeignKey("dbo.Doctor", t => t.DoctorId)
                .Index(t => t.DoctorId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.FollowUp",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Systolic = c.String(),
                        Diastolic = c.String(),
                        HeartRate = c.String(),
                        Temprature = c.String(),
                        Title = c.String(),
                        Subjective = c.String(),
                        Objective = c.String(),
                        Assessment = c.String(),
                        Plan = c.String(),
                        Cost = c.String(),
                        Paid = c.String(),
                        AdditionalInformation = c.String(),
                        ConsultationId = c.Guid(nullable: false),
                        ClinicId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinic", t => t.ClinicId)
                .ForeignKey("dbo.Consultation", t => t.ConsultationId)
                .Index(t => t.ConsultationId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsExpired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClinicAssistant",
                c => new
                    {
                        Clinic_Id = c.Guid(nullable: false),
                        Assistant_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Clinic_Id, t.Assistant_Id })
                .ForeignKey("dbo.Clinic", t => t.Clinic_Id, cascadeDelete: true)
                .ForeignKey("dbo.Assistant", t => t.Assistant_Id, cascadeDelete: true)
                .Index(t => t.Clinic_Id)
                .Index(t => t.Assistant_Id);
            
            CreateTable(
                "dbo.DoctorAssistant",
                c => new
                    {
                        Doctor_Id = c.Guid(nullable: false),
                        Assistant_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Doctor_Id, t.Assistant_Id })
                .ForeignKey("dbo.Doctor", t => t.Doctor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Assistant", t => t.Assistant_Id, cascadeDelete: true)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Assistant_Id);
            
            CreateTable(
                "dbo.DoctorClinic",
                c => new
                    {
                        Doctor_Id = c.Guid(nullable: false),
                        Clinic_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Doctor_Id, t.Clinic_Id })
                .ForeignKey("dbo.Doctor", t => t.Doctor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Clinic", t => t.Clinic_Id, cascadeDelete: true)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Clinic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointment", "PatientId", "dbo.Patient");
            DropForeignKey("dbo.FollowUp", "ConsultationId", "dbo.Consultation");
            DropForeignKey("dbo.FollowUp", "ClinicId", "dbo.Clinic");
            DropForeignKey("dbo.Patient", "DoctorId", "dbo.Doctor");
            DropForeignKey("dbo.Consultation", "PatientId", "dbo.Patient");
            DropForeignKey("dbo.Patient", "ClinicId", "dbo.Clinic");
            DropForeignKey("dbo.Consultation", "DoctorId", "dbo.Doctor");
            DropForeignKey("dbo.DoctorClinic", "Clinic_Id", "dbo.Clinic");
            DropForeignKey("dbo.DoctorClinic", "Doctor_Id", "dbo.Doctor");
            DropForeignKey("dbo.DoctorAssistant", "Assistant_Id", "dbo.Assistant");
            DropForeignKey("dbo.DoctorAssistant", "Doctor_Id", "dbo.Doctor");
            DropForeignKey("dbo.Appointment", "DoctorId", "dbo.Doctor");
            DropForeignKey("dbo.Consultation", "ClinicId", "dbo.Clinic");
            DropForeignKey("dbo.ClinicAssistant", "Assistant_Id", "dbo.Assistant");
            DropForeignKey("dbo.ClinicAssistant", "Clinic_Id", "dbo.Clinic");
            DropForeignKey("dbo.Appointment", "ClinicId", "dbo.Clinic");
            DropForeignKey("dbo.Appointment", "AssistantId", "dbo.Assistant");
            DropIndex("dbo.DoctorClinic", new[] { "Clinic_Id" });
            DropIndex("dbo.DoctorClinic", new[] { "Doctor_Id" });
            DropIndex("dbo.DoctorAssistant", new[] { "Assistant_Id" });
            DropIndex("dbo.DoctorAssistant", new[] { "Doctor_Id" });
            DropIndex("dbo.ClinicAssistant", new[] { "Assistant_Id" });
            DropIndex("dbo.ClinicAssistant", new[] { "Clinic_Id" });
            DropIndex("dbo.FollowUp", new[] { "ClinicId" });
            DropIndex("dbo.FollowUp", new[] { "ConsultationId" });
            DropIndex("dbo.Patient", new[] { "ClinicId" });
            DropIndex("dbo.Patient", new[] { "DoctorId" });
            DropIndex("dbo.Consultation", new[] { "ClinicId" });
            DropIndex("dbo.Consultation", new[] { "DoctorId" });
            DropIndex("dbo.Consultation", new[] { "PatientId" });
            DropIndex("dbo.Appointment", new[] { "ClinicId" });
            DropIndex("dbo.Appointment", new[] { "AssistantId" });
            DropIndex("dbo.Appointment", new[] { "DoctorId" });
            DropIndex("dbo.Appointment", new[] { "PatientId" });
            DropTable("dbo.DoctorClinic");
            DropTable("dbo.DoctorAssistant");
            DropTable("dbo.ClinicAssistant");
            DropTable("dbo.User");
            DropTable("dbo.FollowUp");
            DropTable("dbo.Patient");
            DropTable("dbo.Doctor");
            DropTable("dbo.Consultation");
            DropTable("dbo.Clinic");
            DropTable("dbo.Assistant");
            DropTable("dbo.Appointment");
        }
    }
}
