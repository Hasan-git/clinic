using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Repositories;


namespace Clinic.Infrastructure.Data.Repositories
{
    public class ClinicRepository : EFRepository<Core.Domain.Models.Clinic>, IClinicRepository
    {
        public ClinicRepository(DbContext dbContext) :
               base(dbContext)
        {
            
        }
        //Get All Clinics
        public new async Task<List<Core.Domain.Models.Clinic>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        //Get an Clinic
        public new async Task<Core.Domain.Models.Clinic> GetById(Guid id)
        {
            var clinic = await DbSet.FirstOrDefaultAsync(p => p.Id == id);
            return clinic;
        }
        public async Task<List<Core.Domain.Models.Clinic>> GetDoctorClinics(Guid id)
        {
             var clinics = await DbSet.Where(u => u.Doctors.Any(e => e.Id == id)).ToListAsync();
            
            return clinics;
        }

    }
    
}
