using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Common.Core.Dates;

namespace Clinic.Core.Domain.Models
{
    public class Clinic : IBaseModel, IEquatable<Clinic>
    {
        public Clinic()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            Doctors = new HashSet<Doctor>();
            Assistants = new HashSet<Assistant>();
            Consultations = new HashSet<Consultation>();
            FollowUps = new HashSet<FollowUp>();
            Appointments = new HashSet<Appointment>();
            Patients = new HashSet<Patient>();
        }


        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Assistant> Assistants { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }

        public bool Equals(Clinic other)
        {
            return Id.Equals(other.Id);
        }
    }
}
