using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dal.Core.Repository
{
    public class RepositoryException:Exception
    {
        public RepositoryException(MethodBase methodBase, Exception exception)
            : base(exception.Message, exception)
        {
            Method = methodBase.Name;
            Class = methodBase.DeclaringType != null ? methodBase.DeclaringType.FullName : null;
        }

        public string Class { get; }
        public string Method { get; }
    }
}
