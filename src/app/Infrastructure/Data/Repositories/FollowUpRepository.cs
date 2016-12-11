using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;

namespace Clinic.Infrastructure.Data.Repositories
{
    public class FollowUpRepository :  EFRepository<FollowUp>, IFollowUpRepository
    {
        public FollowUpRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
        //Get all Followups
        public new async Task<List<FollowUp>> GetAll()
        {
                return await DbSet.ToListAsync();

        }
        //Get an followUp
        public new async Task<FollowUp> GetById(Guid id)
        {
                return await DbSet.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
