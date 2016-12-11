using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Clinic.Core.Domain.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        new Task<List<Doctor>> GetAll();
        new Task<Doctor> GetById(Guid id);
       
    }
}
