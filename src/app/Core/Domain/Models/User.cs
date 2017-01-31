using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Clinic.Common.Core.Dates;
using System.ComponentModel;

namespace Clinic.Core.Domain.Models
{
    public class User : IBaseModel, IEquatable<User>
    {
        public User()
        {
            CreatedDate = DomainTime.Now();
            ModifiedDate = DomainTime.Now();
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
       

        public bool Equals(User other)
        {
            return Id.Equals(other.Id);
        }
    }
}
