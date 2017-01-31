using Clinic.Common.Core.Dates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Domain.Models
{
    public class MedicalStatus : IBaseModel, IEquatable<MedicalStatus>
    {
        public MedicalStatus()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();

        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Diseases { get; set; }
        public string Allergies { get; set; }
        public string SurgicalHistory { get; set; }
        public string PastMedication { get; set; }
        public string PresentMedication { get; set; }

        //public Guid PatientId { get; set; }

        //[ForeignKey("PatientId")]
        //public virtual Patient Patient { get; set; }

        public bool Equals(MedicalStatus other)
        {
            return Id.Equals(other.Id);
        }

    }
}
