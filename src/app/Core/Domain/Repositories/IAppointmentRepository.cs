using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;

namespace Clinic.Core.Domain.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        new Task<List<Appointment>> GetAll();
        Task<List<Appointment>> GetAppointments();
        new Task<Appointment> GetById(Guid id);
        Task<List<Appointment>> GetByDoctorId(Guid id);

        Task<Appointment> GetBypatientId(Guid id);
    }
}
