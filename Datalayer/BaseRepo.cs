using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datalayer.Models.BaseClass;
using Microsoft.IdentityModel.Tokens;
namespace Datalayer
{
    public class BaseRepo<T> where T : BaseEntity
    {
        private  DbSet<T> _dbSet;
        private List<T> _items;
        public BaseRepo()
        {
        }
        public void InitialiseItems(RelocationDbContext context)
        {
            _dbSet = context.Set<T>();
            _items = _dbSet.Select(item => item).ToListAsync().Result;
        }
        public async Task<T> Add(T item)
        {
            try
            {
                _dbSet.Add(item);
                _items.Add(item);
                await _dbSet.GetDbContext().SaveChangesAsync();
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException?.Message); }
            return item;
        }
        public List<T> GetItems()
            => _items;
        public async Task<T> Update(T item)
        {
            try
            {
                T? found = _items.Find(item1 => item1.Id == item.Id);
                _dbSet.GetDbContext().Entry(found).CurrentValues.SetValues(item);
                _dbSet.Update(found);
                await _dbSet.GetDbContext().SaveChangesAsync(); 
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException?.Message); }
            return item;
        }
        public async Task<bool> Delete(T item)
        {
            try
            {
                _dbSet.Remove(item);
                _items.Remove(item);
               await _dbSet.GetDbContext().SaveChangesAsync();
            }
            catch (Exception ex) { Console.WriteLine(ex.InnerException?.Message); return false; }
            return true;
        }
        public async Task<bool> DeleteAll(List<T> items)
        {
            if (items == null)
                return false;
            items.ForEach(item => _dbSet.Remove(item));
            await _dbSet.GetDbContext().SaveChangesAsync();
            return true;
        }
        public async Task<List<T>> UpdateAll(List<T> items)
        {
            foreach(T item in items)
            {
                T? found = _items.Find(item1 => item1.Id == item.Id);
                _dbSet.GetDbContext().Entry(found).CurrentValues.SetValues(item);
                _dbSet.Update(found);
            }
            await _dbSet.GetDbContext().SaveChangesAsync();
            return items;
        }
    }
}
