
using System.Composition;
using System.Data;
using System.Data.SqlClient;

namespace BusinessUnit.Dal.Core
{
    [Export(typeof(IConnectionProvider))]
    public class ConnectionProvider : IConnectionProvider
    {
        public virtual IDbConnection GetConnection()
        {
            //Northwind
            var connection = new SqlConnection("server=localhost;database=Northwind;Integrated Security=True");
            connection.Open();
            return connection;
        }
    }
}
