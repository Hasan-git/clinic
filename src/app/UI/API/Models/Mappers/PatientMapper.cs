using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;

namespace Api.Models.Mappers
{
    public class PatientMapper : IPatientMapper
    {
        public MedicalStatus MapMedicalStatus(Patient model, MedicalStatus medicalStatus)
        {
            //var medicalStatus = _uow.MedicalRepository.GetById(model.MedicalStatus.Id);
            //var medicalStatus = new MedicalStatus();

            medicalStatus.Allergies = string.IsNullOrEmpty(model.MedicalStatus.Allergies) ? medicalStatus.Allergies : medicalStatus.Allergies + "\n" + model.MedicalStatus.Allergies;
            medicalStatus.PastMedicalHistory = string.IsNullOrEmpty(model.MedicalStatus.PastMedicalHistory) ? medicalStatus.PastMedicalHistory : medicalStatus.PastMedicalHistory + "\n" + model.MedicalStatus.PastMedicalHistory;
            medicalStatus.PastMedication = string.IsNullOrEmpty(model.MedicalStatus.PastMedication) ? medicalStatus.PastMedication : medicalStatus.PastMedication + "\n" + model.MedicalStatus.PastMedication;
            medicalStatus.PresentMedication = string.IsNullOrEmpty(model.MedicalStatus.PresentMedication) ? medicalStatus.PresentMedication : medicalStatus.PresentMedication + "\n" + model.MedicalStatus.PresentMedication;
            medicalStatus.SurgicalHistory = string.IsNullOrEmpty(model.MedicalStatus.SurgicalHistory) ? medicalStatus.SurgicalHistory : medicalStatus.SurgicalHistory + "\n" + model.MedicalStatus.SurgicalHistory;

            return medicalStatus;
        }
    }

    
}