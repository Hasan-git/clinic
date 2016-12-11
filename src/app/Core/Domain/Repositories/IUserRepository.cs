using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Clinic.Core.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        new Task<List<User>> GetAll();
        Task<List<User>> GetAllDoctors();
        Task<List<User>> GetAllAssistants();
        Task<User> GetDoctorById(Guid id);
        Task<User> GetAssistantById(Guid id);
        Task<IdentityResult> RegisterUser(User user);

    }
}
