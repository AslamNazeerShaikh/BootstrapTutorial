using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapTutorial.WebUi.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<int> AddAsync(T entity);
        public Task<int> UpdateAsync(T entity);
        public Task<int> DeleteAsync(int id);
    }
}
