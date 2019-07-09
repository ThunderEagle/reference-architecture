using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dal.Core.Repository
{
	public class Repository<T>:IRepository<T> where T : class
	{
		private readonly Func<IDbConnection> _connectionFactory;

		public Repository(Func<IDbConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}


		public IEnumerable<T> List => throw new NotImplementedException();

		public void Add(T entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(T entity)
		{
			throw new NotImplementedException();
		}

		public T Get(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(T entity)
		{
			throw new NotImplementedException();
		}
	}
}
