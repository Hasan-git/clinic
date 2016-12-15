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
    public class ConsultationRepository :  EFRepository<Consultation>, IConsultationRepository
    {
        public ConsultationRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
        //Get All FollowUps 
        public new async Task<List<Consultation>> GetAll()
        {
                return await DbSet.Include(c=>c.FollowUps).Include(c => c.Images).ToListAsync();
        }
        //Get All Doctor's Consultation 
        public async Task<List<Consultation>> GetAllByDoctorId(Guid doctorId)
        {
                var consultations = await DbSet.Include(x=>x.FollowUps).Include(x => x.Images).Where(p => p.DoctorId == doctorId).ToListAsync();
                if (consultations == null)
                    return null;

                return consultations;
        }

       // Get specific doctors's consultation by consultation Id
        public new async Task<Consultation> GetById(Guid consultationId)
        {
                var consultation = await DbSet.Include(f=>f.FollowUps).Include(x => x.Images).FirstOrDefaultAsync(p => p.Id == consultationId );
                if (consultation == null)
                    return null;

                return consultation;
        }

        //Get all consultations for doctor's patient by patient Id
        public async Task<List<Consultation>> GetByPatientId(Guid doctorId, Guid patientId)
        {
            var consultations = await DbSet.Include(f => f.FollowUps).Include(x => x.Images).Where(p => p.PatientId == patientId && p.DoctorId == doctorId).ToListAsync();
            if (consultations == null)
                return null;

            return consultations;
        }
    }
}
