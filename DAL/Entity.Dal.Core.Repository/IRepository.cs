using System.Collections.Generic;

namespace Entity.Dal.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List { get; }
        int Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T Get(int id);
    }
}
