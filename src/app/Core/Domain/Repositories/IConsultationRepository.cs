using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;

namespace Clinic.Core.Domain.Repositories
{
    public interface IConsultationRepository : IRepository<Consultation>
    {
        new Task<List<Consultation>> GetAll();
        Task<List<Consultation>> GetAllByDoctorId(Guid doctorId);

        //Get specific doctors's consultation by consultation Id
        new Task<Consultation> GetById(Guid consultationId);

        //Get all consultations for doctor's patient by patient Id
        Task<List<Consultation>> GetByPatientId( Guid doctorId,Guid patientId);

    }
}
