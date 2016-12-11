using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;


namespace Clinic.Core.Domain.Repositories
{
    public interface IClinicRepository : IRepository<Models.Clinic>
    {
        new Task<List<Models.Clinic>> GetAll();
        new Task<Models.Clinic> GetById(Guid id);

        //Get doctor's Clinic , By doctor Id
        Task <List<Models.Clinic>> GetDoctorClinics(Guid id);
    }
}
