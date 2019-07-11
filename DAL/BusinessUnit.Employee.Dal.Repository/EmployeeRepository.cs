using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Dapper;
using Entity.Dal.Core.Repository;

namespace Entity.Employee.Dal.Repository
{
    public class EmployeeRepository:Repository<Entities.Employee>
    {
        public EmployeeRepository(Func<IDbConnection> connectionFactory)
            :base(connectionFactory) { }

        public override int Add(Entities.Employee entity)
        {
            try
            {
                var sql = "INSERT INTO Employees (LastName, FirstName, BirthDate) VALUES (@LastName, @FirstName, @BirthDate)";
                var parms = new DynamicParameters(new { entity.LastName, entity.FirstName, entity.BirthDate });
                using(var cn = ConnectionFactory.Invoke())
                {
                    var key = cn.ExecuteScalar<int>(sql, parms);
                    return key;
                }
            }
            catch(RepositoryException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new RepositoryException(MethodBase.GetCurrentMethod(), e);
            }
        }

        public override void Delete(Entities.Employee entity)
        {
            try
            {
                var sql = "DELETE FROM Employees where EmployeeID = @EmployeeID";
                var parms = new DynamicParameters(new { entity.EmployeeID });
                using(var cn = ConnectionFactory.Invoke())
                {
                    cn.Execute(sql, parms);
                }
            }
            catch(RepositoryException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new RepositoryException(MethodBase.GetCurrentMethod(), e);
            }
        }

        public override void Update(Entities.Employee entity)
        {
            try
            {
                var sql = "UPDATE Employees LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate WHERE EmployeeID = @EmployeeID";
                var parms = new DynamicParameters(new { entity.LastName, entity.FirstName, entity.BirthDate, entity.EmployeeID });
                using(var cn = ConnectionFactory.Invoke())
                {
                    var rowCount = cn.Execute(sql, parms);
                }
            }
            catch(RepositoryException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new RepositoryException(MethodBase.GetCurrentMethod(), e);
            }
        }

        public override IEnumerable<Entities.Employee> List
        {
            get
            {
                try
                {
                    var sql = "SELECT * FROM Employees";
                    using(var cn = ConnectionFactory.Invoke())
                    {
                        return cn.Query<Entities.Employee>(sql);
                    }
                }
                catch(RepositoryException)
                {
                    throw;
                }
                catch(Exception e)
                {
                    throw new RepositoryException(MethodBase.GetCurrentMethod(), e);
                }
            }
        }

        public override Entities.Employee Get(int id)
        {
            try
            {
                var parms = new DynamicParameters(new { id });
                var sql = "SELECT EmployeeID, LastName, FirstName, BirthDate FROM Employees WHERE EmployeeID = @id";

                using(var cn = ConnectionFactory.Invoke())
                {
                    return cn.QuerySingleOrDefault<Entities.Employee>(sql, parms);
                }
            }
            catch(RepositoryException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new RepositoryException(MethodBase.GetCurrentMethod(), e);
            }
        }
    }
}