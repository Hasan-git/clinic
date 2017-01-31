using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;

namespace Api.Models.Mappers
{
    public class ConsultationMapper : IConsultationMapper
    {
        private readonly IUnitOfWork _uow;

        public ConsultationMapper(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Consultation mapConsultation(ConsultationModel model)
        {
            var consultation = new Consultation();

            consultation.Id = Guid.NewGuid();
            consultation.AdditionalInformation = model.AdditionalInformation;
            consultation.ChiefComplaint = model.ChiefComplaint;
            consultation.ClinicId = model.ClinicId;
            consultation.Condition = model.Condition;
            consultation.Consultations = model.Consultations;
            consultation.Diagnosis = model.Diagnosis;
            consultation.DifferentialDiagnosis = model.DifferentialDiagnosis;
            consultation.DoctorId = model.DoctorId;
            consultation.EntryDate = model.EntryDate;
            consultation.IsDeleted = false;
            consultation.Lab = model.Lab;
            consultation.Medication = model.Medication;
            consultation.Other = model.Other;
            consultation.PastHistory = model.PastHistory;
            consultation.PatientId = model.PatientId;
            consultation.PhysicalExam = model.PhysicalExam;
            consultation.PresentHistory = model.PresentHistory;
            consultation.Radiology = model.Radiology;
            consultation.Surgery = model.Surgery;

            return consultation;

        }

        public MedicalStatus MapMedicalStatus(ConsultationModel model)
        {
            var medicalStatus = _uow.MedicalRepository.GetById(model.MedicalStatus.Id);

            medicalStatus.Allergies = model.MedicalStatus.Allergies;
            medicalStatus.Diseases = model.MedicalStatus.Diseases;
            medicalStatus.PastMedication = model.MedicalStatus.PastMedication;
            medicalStatus.PresentMedication = model.MedicalStatus.PresentMedication;
            medicalStatus.SurgicalHistory = model.MedicalStatus.SurgicalHistory ;

            return medicalStatus;
        }
    }
}