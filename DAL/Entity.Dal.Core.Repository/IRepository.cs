using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dal.Core.Repository
{
	public interface IRepository<T> where T: class
	{
		IEnumerable<T> List { get; }
		void Add(T entity);
		void Delete(T entity);
		void Update(T entity);
		T Get(int id);
	}
}
