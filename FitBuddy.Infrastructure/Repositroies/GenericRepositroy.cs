using FitBuddy.Core.Repositroy.Contract;
using FitBuddy.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Infrastructure.Repositroies
{
    public class GenericRepositroy<T> : IGenericRepositroy<T> where T : class
    {
        private readonly StoreDbContext _store;

        public GenericRepositroy(StoreDbContext store)
        {
            _store = store;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _store.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _store.Set<T>().FindAsync(id);
        }

        public void Add(T item) {
            _store.Add(item);
            _store.SaveChanges();
        }

        public void Update(T item)
        {   
            _store.Update(item);
            _store.SaveChanges();
        }

        public void Delete(T item)
        {
            _store.Remove(item);
            _store.SaveChanges();
        }
    }
}
