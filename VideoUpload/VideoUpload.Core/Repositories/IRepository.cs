﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Repositories
{
    public interface IRepository<T> where T : class 
    {
        List<T> GetAll();
        T GetById(object id);
        void Add(T entity);
        void Remove(T entity);       
            
    }
}
