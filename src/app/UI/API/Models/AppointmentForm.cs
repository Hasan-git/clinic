using System;

namespace Api.Models
{
    public class AppointmentForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Added { get; set; }
        public bool AllDay { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public bool ExistingPatient { get; set; }
        public int Mobile { get; set; }
        public string Description { get; set; }



    }
}