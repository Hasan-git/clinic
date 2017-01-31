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
    public class PatientRepository :EFRepository<Patient>,IPatientRepository
    {
        public PatientRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
        //public  List<Patient> GetAllPatients()
        //{
        //   return DbSet.Include(x => x.Doctor).ToList();
        //}
        public new async Task<List<Patient>> GetAll()
        {
            return await DbSet.Include(x=>x.MedicalStatus).ToListAsync();
        }
        //Get patients by doctor
        public async Task<List<Patient>> GetByDoctorId(Guid id)
        {
                var patients = await DbSet.Include(x=>x.MedicalStatus).Where(p => p.DoctorId == id & p.IsDeleted == false).ToListAsync();
                if (patients == null)
                    return null;

                return patients;
        }
        //Get an patient 
        public new async Task<Patient> GetById(Guid id)
        {
                Patient patient = await DbSet.Include(x=>x.MedicalStatus).FirstOrDefaultAsync(p => p.Id == id);
                if (patient == null)
                         return null;

                return patient;
        }

        

    }
}
