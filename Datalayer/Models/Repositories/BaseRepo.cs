using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Repositories
{
    public class BaseRepo<T> where T : class
    {
        protected readonly RelocationDbContext _relocationDbContext;
        private readonly DbSet<T> _dbSet;
        public BaseRepo(RelocationDbContext context)
        {
            _relocationDbContext = context;
            _dbSet = context.Set<T>();
        }
        public void Add(T item)
            =>_dbSet.Add(item);
        public List<T> GetItems()
            => _dbSet.Select(item=> item).ToList();
        public void Update(T item)
            =>_dbSet.Update(item);
        public void Delete(T item)
            =>_dbSet.Remove(item);

    }
}
