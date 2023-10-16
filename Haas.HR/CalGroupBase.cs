using Haas.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    /// <summary>
    /// Base class for creating Cal Group Integrations
    /// </summary>
    public abstract class CalGroupBase : HRDataSourceBase
    {
        public override IList<IEmployee> GetDestinationEmployees()
        {
            throw new NotImplementedException();
        }

        public override IEmployee UpdateDestinationEmployee(IEmployee employee)
        {
            throw new NotImplementedException();
        }


        public override IEmployee AddDestinationEmployee(IEmployee employee)
        {
            throw new NotImplementedException();
        }

        public override IEmployee AddSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
        {
            throw new NotImplementedException();
        }

        public override IEmployee DeleteDestinationEmployee(IEmployee employee)
        {
            throw new NotImplementedException();
        }

        public override IEmployee DeleteSourceEmployee(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }


        public override IList<IEmployee> GetSourceEmployees(IHRDataSourceConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        public override IEmployee UpdateSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
        {
            throw new NotImplementedException();
        }


    }
}
