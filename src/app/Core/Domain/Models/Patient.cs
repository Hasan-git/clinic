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
    public class Patient : IBaseModel, IEquatable<Patient>
    {
        public Patient()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            Consultations = new HashSet<Consultation>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string MartialStatus { get; set; }
        public string InsuranceCompany { get; set; }
        public string Occupation { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string BloodType { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Referrer { get; set; }
        public string AdditionalInformation { get; set; }

        public Guid MedicalStatusId { get; set; }

        [ForeignKey("MedicalStatusId")]
        public virtual MedicalStatus MedicalStatus { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } 
        public Guid DoctorId { get; set; }
        public Guid ClinicId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }


        public virtual ICollection<Consultation> Consultations { get; set; }

        public bool Equals(Patient other)
        {
            return Id.Equals(other.Id);
        }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return FirstName + " " + MiddelName + " " + LastName;
            }
        }
    }
}
