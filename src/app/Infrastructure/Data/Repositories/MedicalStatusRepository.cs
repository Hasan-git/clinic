using Clinic.Core.Domain.Models;
using Clinic.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Data.Repositories
{
    public class MedicalStatusRepository : EFRepository<MedicalStatus>, IMedicalStatusRepository
    {
        public MedicalStatusRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
    }
}
