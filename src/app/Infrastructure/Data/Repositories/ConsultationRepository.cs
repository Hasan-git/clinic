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
                var consultation = await DbSet.Include(f=>f.FollowUps).Include(x=>x.FollowUps.Select(e=>e.Images)).Where(f=>f.IsDeleted==false).Include(x => x.Images).FirstOrDefaultAsync(p => p.Id == consultationId );
                if (consultation == null)
                    return null;

                return consultation;
        }

        //Get all consultations for doctor's patient by patient Id
        public async Task<List<Consultation>> GetByPatientId(Guid doctorId, Guid patientId)
        {
            var consultations = await DbSet
                .Where(p => p.PatientId == patientId && p.DoctorId == doctorId && p.IsDeleted == false)
                .Include( f => f.FollowUps)
                .Include( f => f.FollowUps.Select(c=>c.Images))
                .Include(x => x.Images)
                .OrderBy(u => u.EntryDate)
                .ToListAsync();

            foreach (var consultation in consultations)
            {
                consultation.FollowUps = consultation.FollowUps.OrderBy(u => u.EntryDate).ToList();
            }

            if (consultations == null)
                return null;

            return consultations;
        }

        public async Task<Consultation> GetLastConditionByPatientId(Guid patientId)
        {
            var consultation = await DbSet
                .Where(p => p.PatientId == patientId && p.IsDeleted == false)
                .Include(f => f.FollowUps)
                .OrderByDescending(c => c.EntryDate)
                .FirstOrDefaultAsync()
                ;

            //var consultations2 = await DbSet
            //    .Where(p => p.PatientId == patientId && p.IsDeleted == false)
            //    .Include(f => f.FollowUps)
            //    .Select(x=>x.FollowUps.OrderByDescending(o=>o.EntryDate).FirstOrDefault())
            //    //.FirstOrDefaultAsync()
            //    .ToListAsync()
            //    ;

            return consultation;
        }
    }
}
