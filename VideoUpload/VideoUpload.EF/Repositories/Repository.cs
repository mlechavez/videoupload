using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _set;

        protected DbSet<T> Set
        {
            get { return _set ?? (_set = _context.Set<T>());  }
        }
        public Repository(DbContext context)
        {
            _context = context;
            
        }
        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public List<T> GetAll()
        {
            return Set.ToList();
        }

        public T GetById(object id)
        {
            return Set.Find(id);
        }

        public void Remove(T entity)
        {
            Set.Remove(entity);
        }
    }
}
