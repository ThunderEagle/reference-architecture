using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessUnit.Dal.Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {

        protected Repository(IConnectionProvider connectionProvider)
        {
            ConnectionFactory = connectionProvider.GetConnection;
        }

        protected Func<IDbConnection> ConnectionFactory { get;  }

        public abstract IEnumerable<T> List { get; }

        public abstract int Add(T entity);

        public abstract void Delete(T entity);

        public abstract T Get(int id);

        public abstract void Update(T entity);
    }
}
