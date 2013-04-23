using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using photoshare.Models;

namespace photoshare.Interfaces
{
    public interface IDalRepository<T>
    {
        IEnumerable<T> All();
        T Get(string id);
        T Add(T t);
        void Delete(T t);
        void Update(T t);
    }
}
