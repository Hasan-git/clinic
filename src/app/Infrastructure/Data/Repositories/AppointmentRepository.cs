using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;
using System.Data.Entity.Core.Objects;

namespace Clinic.Infrastructure.Data.Repositories
{
    public class AppointmentRepository :  EFRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
        public new async Task<List<Appointment>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        //Get all Doctor's Appointments 
        public async Task<List<Appointment>> GetByDoctorId(Guid id)
        {
                var appointments = await DbSet.Where(p => p.DoctorId == id).ToListAsync();
                return appointments;
        }

        // returns appointment that have the same patientId and same date 
        public async Task<Appointment> GetBypatientId(Guid id)
        {
            //p.Start.Date == DateTime.Today.Date
            var appointment = await DbSet.Where(p => p.PatientId == id && p.Start.Year == DateTime.Now.Year && p.Start.Month == DateTime.Now.Month && p.Start.Day == DateTime.Now.Day).FirstOrDefaultAsync();

            return appointment;
        }

        //Get an Appointment
        public new async Task<Appointment> GetById(Guid id)
        {
                var appointment = await DbSet.FirstOrDefaultAsync(p => p.Id == id);
                return appointment;
        }
    }
}
