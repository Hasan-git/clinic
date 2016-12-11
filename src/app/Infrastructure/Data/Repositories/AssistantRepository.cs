using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Clinic.Infrastructure.Data.Repositories
{
    public class AssistantRepository : EFRepository<Assistant>, IAssistantRepository
    {
        

        public AssistantRepository(DbContext dbContext) :
            base(dbContext)
        {
            
        }

        public new async Task<List<Assistant>> GetAll()
        {
           return await DbSet.ToListAsync();
          
        }

        public new async Task<Assistant> GetById(Guid id)
        {
            var assistant = await DbSet.FirstOrDefaultAsync(p => p.Id == id );
            return assistant;
          
        }
    }
}

