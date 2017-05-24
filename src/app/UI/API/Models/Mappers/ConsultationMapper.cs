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
        //private readonly IUnitOfWork _uow;

        //public ConsultationMapper(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //}
        public Consultation mapConsultation(ConsultationModel model)
        {
            var consultation =new Consultation();

            consultation.Id = Guid.NewGuid();
            consultation.IsDeleted = false;
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
            consultation.PatientId = model.PatientId;
            consultation.PhysicalExam = model.PhysicalExam;
            consultation.PresentHistory = model.PresentHistory;
            consultation.Radiology = model.Radiology;
            consultation.Surgery = model.Surgery;

            return consultation;

        }

        public Consultation mapConsultation(ConsultationModel model, Consultation consultation)
        {

            consultation.AdditionalInformation = model.AdditionalInformation;
            consultation.ChiefComplaint = model.ChiefComplaint;
            consultation.ClinicId = model.ClinicId;
            consultation.Condition = model.Condition;
            consultation.Consultations = model.Consultations;
            consultation.Diagnosis = model.Diagnosis;
            consultation.DifferentialDiagnosis = model.DifferentialDiagnosis;
            consultation.DoctorId = model.DoctorId;
            consultation.EntryDate = model.EntryDate;
            consultation.Lab = model.Lab;
            consultation.Medication = model.Medication;
            consultation.Other = model.Other;
            consultation.PatientId = model.PatientId;
            consultation.PhysicalExam = model.PhysicalExam;
            consultation.PresentHistory = model.PresentHistory;
            consultation.Radiology = model.Radiology;
            consultation.Surgery = model.Surgery;

            return consultation;

        }
        public MedicalStatus MapMedicalStatus(ConsultationModel model,MedicalStatus medicalStatus)
        {
            //var medicalStatus = _uow.MedicalRepository.GetById(model.MedicalStatus.Id);
            //var medicalStatus = new MedicalStatus();

            medicalStatus.Allergies = string.IsNullOrEmpty(model.MedicalStatus.Allergies) ? medicalStatus.Allergies : medicalStatus.Allergies + "\n" + model.MedicalStatus.Allergies;
            medicalStatus.PastMedicalHistory = string.IsNullOrEmpty(model.MedicalStatus.PastMedicalHistory)  ? medicalStatus.PastMedicalHistory :  medicalStatus.PastMedicalHistory + "\n" + model.MedicalStatus.PastMedicalHistory;
            medicalStatus.PastMedication = string.IsNullOrEmpty(model.MedicalStatus.PastMedication) ? medicalStatus.PastMedication : medicalStatus.PastMedication + "\n" + model.MedicalStatus.PastMedication;
            medicalStatus.PresentMedication = string.IsNullOrEmpty(model.MedicalStatus.PresentMedication) ? medicalStatus.PresentMedication : medicalStatus.PresentMedication + "\n" + model.MedicalStatus.PresentMedication;
            medicalStatus.SurgicalHistory = string.IsNullOrEmpty(model.MedicalStatus.SurgicalHistory) ? medicalStatus.SurgicalHistory : medicalStatus.SurgicalHistory + "\n" + model.MedicalStatus.SurgicalHistory;

            return medicalStatus;
        }
    }
}