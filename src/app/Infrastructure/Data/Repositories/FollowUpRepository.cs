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
                return await DbSet.Include(x => x.Images).ToListAsync();

        }
        //Get an followUp
        public new async Task<FollowUp> GetById(Guid id)
        {
                return await DbSet.Include(x => x.Images).FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<FollowUp> GetLastFollowUpByPatientId(Guid patientId)
        {
            var db = DbContext.Set<Consultation>();

            var followUp = await db
                .Where(p => p.PatientId == patientId && p.IsDeleted == false)
                .Include(f => f.FollowUps)
                .Select(x => x.FollowUps.OrderByDescending(o => o.EntryDate).FirstOrDefault())
                .FirstOrDefaultAsync()
                ;

            return followUp;
        }

    }
}
