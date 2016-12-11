using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Core.Domain;
using Clinic.Core.Domain.Repositories;
using Clinic.Infrastructure.Helpers;

namespace Clinic.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ClinicContext DbContext { get; set; }
        public UnitOfWork(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }
        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public virtual async Task Commit()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
           await DbContext.SaveChangesAsync();
        }

        public void RollBack()
        {
            var context = DbContext;
            var changedEntries = context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }
        }
        protected void CreateDbContext()
        {
            DbContext = new ClinicContext();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = true;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }
        public IAppointmentRepository AppointmentRepository { get { return GetRepo<IAppointmentRepository>(); } }
        public IAssistantRepository AssistantRepository { get { return GetRepo<IAssistantRepository>(); } }
        public IClinicRepository ClinicRepository { get { return GetRepo<IClinicRepository>(); } }
        public IConsultationRepository ConsultationRepository { get { return GetRepo<IConsultationRepository>(); } }
        public IDoctorRepository DoctorRepository { get { return GetRepo<IDoctorRepository>(); } }
        public IFollowUpRepository FollowUpRepository { get { return GetRepo<IFollowUpRepository>(); } }
        public IPatientRepository PatientRepository { get { return GetRepo<IPatientRepository>(); } }
        public IUserRepository UserRepository { get { return GetRepo<IUserRepository>(); } }




        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class, IBaseModel
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}