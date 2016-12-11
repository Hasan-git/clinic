using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Common.Core;
using Clinic.Common.Core.Extensions;
using Clinic.Core.Domain;
using Clinic.Core.Domain.Repositories;

namespace Clinic.Infrastructure.Data.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class, IBaseModel
    {
        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
            dbContext.Configuration.LazyLoadingEnabled = true;
            DbSet = DbContext.Set<T>();
        }

        public DbConnection OpenConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            return OpenConnection(connectionString);
        }
        public DbConnection OpenConnection(string connectionString)
        {
            var cnn = new SqlConnection(connectionString);
            // wrap the connection with a profiling connection that tracks timings 
            //var connection = new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler.Current);

            cnn.OpenWithRetry();
            return cnn;
        }
        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public async Task<List<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public  IQueryable<T> GetAll(int page, int size, out int total)
        {
            if (page < 1) page = 1;
            var start = (page - 1) * size;
            total = DbSet.Count();
            return DbSet
                   .OrderByDescending(x => x.ModifiedDate)
                   .Skip(start)
                   .Take(size);
        }

        public IQueryable<T> GetAll(DateTime? startDate, DateTime? endDate, int page, int size, out int total)
        {
            if (page < 1) page = 1;
            var start = (page - 1) * size;
            total = DbSet.Count();
            
            if (startDate.HasValue && endDate.HasValue)
            {
                return DbSet.OrderByDescending(e => e.CreatedDate).Where(e => e.CreatedDate >= startDate && e.CreatedDate <= endDate).Skip(start).Take(size);
            }
            else if (startDate.HasValue)
            {
                return DbSet.OrderByDescending(e => e.CreatedDate).Where(e => e.CreatedDate >= startDate).Skip(start).Take(size);
            }
            else if (endDate.HasValue)
            {
                return DbSet.OrderByDescending(e => e.CreatedDate).Where(e => e.CreatedDate <= endDate).Skip(start).Take(size);
            }
            return DbSet.OrderByDescending(e => e.CreatedDate).Skip(start).Take(size);
        }

        public virtual T GetTop()
        {
            return DbSet.OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
        }

        public T GetById(Guid id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return   DbSet.Find(id);
        }

        public void Add(T entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = SequentialGuid.Generate();

            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;

            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
               DbSet.Add(entity);
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            DbContext.Configuration.AutoDetectChangesEnabled = false;
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            foreach (var entity in entities)
            {
                if (entity.Id == Guid.Empty)
                    entity.Id = SequentialGuid.Generate();

                DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    DbSet.Add(entity);
                }
            }
            DbContext.Configuration.ValidateOnSaveEnabled = true;
            DbContext.Configuration.AutoDetectChangesEnabled = true;
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            entity.ModifiedDate = DateTime.UtcNow;
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public  virtual void Delete(Guid id)
        {
            var entity =  GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }


    }
}