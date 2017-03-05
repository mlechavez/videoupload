using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoUpload.Core.Repositories
{
    public interface IRepository<T> where T : class 
    {
        List<T> GetAll();

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        Task<T> GetByIdAsync(object id, CancellationToken cancellationToken);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);             
    }
}
