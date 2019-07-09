using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dal.Core
{
	public class ConnectionProvider
	{
		private SQLiteConnection _db = new SQLiteConnection(":memory:");

		public ConnectionProvider()
		{

		}

		public virtual IDbConnection GetConnection()
		{
			//TODO can't use this, it will kill the database, either need a file or something else.
			return _db;
		}
	}
}
