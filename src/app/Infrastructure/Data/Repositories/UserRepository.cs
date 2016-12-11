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
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        private UserManager<IdentityUser> _userManager;

        public UserRepository(DbContext dbContext) :
            base(dbContext)
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(dbContext));
        }

        public new async Task<List<User>> GetAll()
        {
            //return await DbSet.Include(x => x.Patients).ToListAsync();
            return null;
        }

        public async Task<List<User>> GetAllAssistants()
        {
            //return await DbSet.Where(x=>x.Type=="assistant").ToListAsync();
            return null;
        }

        public async Task<List<User>> GetAllDoctors()
        {
            //return await DbSet.Where(x => x.Type == "doctor").Include(x=>x.Patients).ToListAsync();
            return null;
        }

        public async Task<User> GetAssistantById(Guid id)
        {
            // var assistant = await DbSet.FirstOrDefaultAsync(p => p.Id == id && p.Type == "assistant");
            //return assistant;
            return null;
        }

        public async Task<User> GetDoctorById(Guid id)
        {
            //var doctor = await DbSet.Include(x=>x.Patients).FirstOrDefaultAsync(p => p.Id == id && p.Type=="doctor");
                //return doctor;
            return null;
        }

        public async Task<IdentityResult> RegisterUser(User user)
        {
            var identity = new IdentityUser
            {
                UserName = user.Username
            };

            var result = await _userManager.CreateAsync(identity, user.Password);

            return result;
        }
    }
}

