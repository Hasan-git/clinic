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
    public class Images : IBaseModel, IEquatable<Images>
    {
        public Images()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(200)]
        public string ImageName { get; set; }

        //[ForeignKey("ConsultaionId")]
        //public virtual Consultation Consultation { get; set; }
        //public Guid ConsultaionId { get; set; }

        //[ForeignKey("FollowUpId")]
        //public virtual FollowUp FollowUp { get; set; }
        //public Guid FollowUpId { get; set; }
        public bool Equals(Images other)
        {
            return Id.Equals(other.Id);
        }
    }
}
