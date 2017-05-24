using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class MedicalStatusModel
    {
        public Guid Id { get; set; }
        public string PastMedicalHistory { get; set; }
        public string Allergies { get; set; }
        public string SurgicalHistory { get; set; }
        public string PastMedication { get; set; }
        public string PresentMedication { get; set; }
    }
}