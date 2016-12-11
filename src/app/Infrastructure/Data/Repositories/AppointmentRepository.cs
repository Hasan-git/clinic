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
        //Get an Appointment
        public new async Task<Appointment> GetById(Guid id)
        {
                var appointment = await DbSet.FirstOrDefaultAsync(p => p.Id == id);
                return appointment;
        }
    }
}
