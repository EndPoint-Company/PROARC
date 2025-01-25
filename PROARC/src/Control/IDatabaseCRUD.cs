using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public interface IDatabaseCRUD<T>
    {
        static async Task<List<T>?> GetAll() { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<T?> Get(int id) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> Insert(T obj) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> Update(int id, T obj) { await Task.CompletedTask; throw new NotImplementedException(); }
        static async Task<bool> Delete(int id) { await Task.CompletedTask; throw new NotImplementedException(); }
    }
}
