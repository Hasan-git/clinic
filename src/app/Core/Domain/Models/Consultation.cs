using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Common.Core.Dates;
using System.ComponentModel;

namespace Clinic.Core.Domain.Models
{
    public class Consultation : IBaseModel, IEquatable<Consultation>
    {
        public Consultation()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            FollowUps = new HashSet<FollowUp>();
            Images = new HashSet<Images>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime EntryDate { get; set; }

        public string Title { get; set; }
        public string Condition { get; set; }
        public string ChiefComplaint { get; set; }
        public string PresentHistory { get; set; }
        //public string PastHistory { get; set; }
        public string PhysicalExam { get; set; }
        public string DifferentialDiagnosis { get; set; }
        public string Lab { get; set; }
        public string Radiology { get; set; }
        public string Consultations { get; set; }
        public string Diagnosis { get; set; }
        public string Medication { get; set; }
        public string Surgery { get; set; }
        public string Recommendation { get; set; }
        public string Other { get; set; }
        public string AdditionalInformation { get; set; }

        //public string FinancialCoverage { get; set; }
        //public string Diastolic { get; set; }
        //public string Systolic { get; set; }
        //public string HeartRate { get; set; }
        //public string Temprature { get; set; }
        //public string Cost { get; set; }
        //public string Paid { get; set; }

        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid ClinicId { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }

        public ICollection<FollowUp> FollowUps { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        

        public bool Equals(Consultation other)
        {
            return Id.Equals(other.Id);
        }
    }
}
