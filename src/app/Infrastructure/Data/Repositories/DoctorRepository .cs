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
    public class DoctorRepository : EFRepository<Doctor>, IDoctorRepository
    {
        

        public DoctorRepository(DbContext dbContext) :
            base(dbContext)
        {
            
        }

        public new async Task<List<Doctor>> GetAll()
        {
           return await DbSet.ToListAsync();
        }

        public new async Task<Doctor> GetById(Guid id)
        {
            var doctor = await DbSet.Include(x => x.Patients).FirstOrDefaultAsync(p => p.Id == id );
            return doctor;
          
        }
    }
}

