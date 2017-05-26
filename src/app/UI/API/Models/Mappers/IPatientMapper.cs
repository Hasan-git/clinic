using Clinic.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models.Mappers
{
    public interface IPatientMapper
    {
        MedicalStatus MapMedicalStatus(Patient model, MedicalStatus medicalStatus_);

    }
}
