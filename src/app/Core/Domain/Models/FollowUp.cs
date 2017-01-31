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
    public class FollowUp : IBaseModel, IEquatable<FollowUp>
    {
        public FollowUp()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
            Images = new HashSet<Images>();

        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime EntryDate { get; set; }

        public string Title { get; set; }
        public string Condition { get; set; }
        public string Subjective { get; set; }
        public string PhysicalExam { get; set; }
        public string Lab { get; set; }
        public string Radiology { get; set; }
        public string Consultations { get; set; }
        public string Assessment { get; set; }
        public string Medication { get; set; }
        public string Surgery { get; set; }
        public string Other { get; set; }
        public string AdditionalInformation { get; set; }


        //public string FinancialCoverage { get; set; }
        //public string Systolic { get; set; }
        //public string Diastolic { get; set; }
        //public string HeartRate { get; set; }
        //public string Temprature { get; set; }
        //public string Plan { get; set; }
        //public string Cost { get; set; }
        //public string Paid { get; set; }
        public Guid ConsultationId { get; set; }
        public Guid ClinicId { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [ForeignKey("ConsultationId")]
        public virtual Consultation Consultation { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }
        public virtual ICollection<Images> Images { get; set; }

        public bool Equals(FollowUp other)
        {
            return Id.Equals(other.Id);
        }
    }
}
