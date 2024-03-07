using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer
{
    public class BaseRepo<T> where T : class
    {
        protected readonly RelocationDbContext _relocationDbContext;
        protected readonly DbSet<T> _dbSet;
        protected List<T> _items;
        public BaseRepo(RelocationDbContext context)
        {
            _relocationDbContext = context;
            _dbSet = context.Set<T>();
            _items = _dbSet.Select(item => item).ToListAsync().Result;
        }
        public async Task<T> Add(T item)
        {
            try
            {
                _dbSet.Add(item);
                _items.Add(item);
                await _relocationDbContext.SaveChangesAsync();
            }
            catch(Exception ex) { Console.WriteLine(ex.InnerException?.Message); }
            return item;
        }
        public List<T> GetItems()
            => _items;
        public async Task<T> Update(T item)
        {
            try
            {
                _dbSet.Update(item);
                await _relocationDbContext.SaveChangesAsync();
            }
            catch(Exception ex) { Console.WriteLine(ex.InnerException?.Message); }
            return item;
        }
        public async Task<bool> Delete(T item)
        {
            try
            {
                _dbSet.Remove(item);
                _items.Remove(item);
                await _relocationDbContext.SaveChangesAsync();
            }
            catch(Exception ex) { Console.WriteLine(ex.InnerException?.Message);return false; }
            return true;
        }
    }
}
