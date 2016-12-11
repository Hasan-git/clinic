using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Common.Core.Dates;

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
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string BloodType { get; set; }
        public string Gender { get; set; }
        public int Mobile { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime EntryDate { get; set; }
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
