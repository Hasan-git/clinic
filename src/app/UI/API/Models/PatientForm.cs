using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class PatientForm
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "First name is required", AllowEmptyStrings = false)]
        [MinLength(3, ErrorMessage = "First name min length is 3 characters")]
        [MaxLength(20, ErrorMessage = "First name max length is 20 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middel name is required", AllowEmptyStrings = false)]
        [MinLength(3, ErrorMessage = "Middel name min length is 3 characters")]
        [MaxLength(20, ErrorMessage = "Middel name max length is 20 characters")]
        public string MiddelName { get; set; }

        [Required(ErrorMessage = "Last name is required", AllowEmptyStrings = false)]
        [MinLength(3, ErrorMessage = "Last name min length is 3 characters")]
        [MaxLength(20, ErrorMessage = "Last name max length is 20 characters")]
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Birthday is required", AllowEmptyStrings = false)]
        public DateTime Birthday { get; set; }
        public string BloodType { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mobile number is required", AllowEmptyStrings = false)]
        public int Mobile { get; set; }
        public int Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime EntryDate { get; set; }
        

    }
}