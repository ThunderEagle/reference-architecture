using System.Data;

namespace BusinessUnit.Dal.Core {
    public interface IConnectionProvider {
        IDbConnection GetConnection();
    }
}