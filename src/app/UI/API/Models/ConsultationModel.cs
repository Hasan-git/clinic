using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ConsultationModel
    {
        public DateTime EntryDate { get; set; }
        public string Title { get; set; }
        public string Condition { get; set; }
        public string ChiefComplaint { get; set; }
        public string PresentHistory { get; set; }
        public string PastHistory { get; set; }
        public string PhysicalExam { get; set; }
        public string DifferentialDiagnosis { get; set; }
        public string Lab { get; set; }
        public string Radiology { get; set; }
        public string Consultations { get; set; }
        public string Diagnosis { get; set; }
        public string Medication { get; set; }
        public string Surgery { get; set; }
        public string Other { get; set; }
        public string AdditionalInformation { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid ClinicId { get; set; }

        public MedicalStatusModel MedicalStatus { get; set; }
    }
}