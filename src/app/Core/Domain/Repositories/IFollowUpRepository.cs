using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;

namespace Clinic.Core.Domain.Repositories
{
    public interface IFollowUpRepository : IRepository<FollowUp>
    {
        new Task<List<FollowUp>> GetAll();
        new Task<FollowUp> GetById(Guid id);
        Task<FollowUp> GetLastFollowUpByPatientId( Guid patientId);


    }
}
