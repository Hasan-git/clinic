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
    public class Assistant : IBaseModel, IEquatable<Assistant>
    {
        public Assistant()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            Clinics = new HashSet<Clinic>();
            Appointments = new HashSet<Appointment>();
            Doctors = new HashSet<Doctor>();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }        
        public string AdditionalInformation { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }

        public bool Equals(Assistant other)
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
