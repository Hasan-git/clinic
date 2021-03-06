﻿using Clinic.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models.Mappers
{
    public interface IConsultationMapper
    {
        Consultation mapConsultation(ConsultationModel model);
        Consultation mapConsultation(ConsultationModel model, Consultation consultation_);
        MedicalStatus MapMedicalStatus(ConsultationModel model,MedicalStatus medicalStatus_);

    }
}
