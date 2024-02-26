using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Code.Interfaces
{
    public interface IRepository<T,Key> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Key id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        bool Add(T item);
        bool Update(T item);
        bool Delete(Key id);
    }
}
