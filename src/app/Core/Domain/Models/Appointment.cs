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
    public class Appointment : IBaseModel
    {
        public Appointment()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
        }
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? Datetime { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public bool? ExistingPatient { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public Guid? PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid? AssistantId { get; set; }
        public Guid ClinicId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        [ForeignKey("AssistantId")]
        public virtual Assistant Assistant { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }
        public bool Equals(Appointment other)
        {
            return Id.Equals(other.Id);
        }
    }
}
