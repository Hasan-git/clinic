using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Clinic.Core.Domain.Repositories
{
    public interface IAssistantRepository : IRepository<Assistant>
    {
        new Task<List<Assistant>> GetAll();
        new Task<Assistant> GetById(Guid id);
        

    }
}
