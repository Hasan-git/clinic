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
    public class ImageRepository  :EFRepository<Images>, IImageRepository
    {
        public ImageRepository(DbContext dbcontext) 
            : base(dbcontext)
        {
        }
        public  async Task<Images> GetImageById(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(f => f.Id == id);
        }

    }
}
