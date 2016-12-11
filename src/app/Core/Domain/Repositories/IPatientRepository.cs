using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;

namespace Clinic.Core.Domain.Repositories
{
    public interface IPatientRepository :IRepository<Patient>
    {
        new Task<List<Patient>> GetAll();
        new Task<Patient> GetById(Guid id);
         Task<List<Patient>> GetByDoctorId(Guid id);

    }
}
