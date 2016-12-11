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
    public class Doctor :IBaseModel, IEquatable<Doctor>
    {
        public Doctor()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            Patients = new HashSet<Patient>();
            Consultations = new HashSet<Consultation>();
            Clinics = new HashSet<Clinic>();
            Appointments = new HashSet<Appointment>();
            Assistants = new HashSet<Assistant>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Mobile { get; set; }
        public int? Phone { get; set; }
        public string Speciality { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Assistant> Assistants { get; set; }

        public bool Equals(Doctor other)
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
