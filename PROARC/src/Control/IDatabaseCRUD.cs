using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public interface IDatabaseCRUD<T>
    {
        static async Task<List<T>?> GetAllAsync() { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<T?> GetAsync(int id) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> InsertAsync(T obj) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> UpdateAsync(int id, T obj) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> DeleteAsync(int id) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<int> CountProcessosAsync() { await Task.CompletedTask; throw new NotImplementedException(); }
    }
}
