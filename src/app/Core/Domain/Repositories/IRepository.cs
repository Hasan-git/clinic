using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Core.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        IQueryable<T> GetAll(int page, int size, out int total);
        IQueryable<T> GetAll(DateTime? startDate, DateTime? endDate, int page, int size, out int total);
        T GetTop();
        T GetById(Guid id);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Guid id);
    }
}