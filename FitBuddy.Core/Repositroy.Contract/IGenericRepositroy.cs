using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitBuddy.Core.Repositroy.Contract
{
    public interface IGenericRepositroy<T>
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
