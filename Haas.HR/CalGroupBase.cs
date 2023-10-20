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
        public override IQueryable<IEntity> GetDestinationEntities()
        {
            throw new NotImplementedException();
        }

        public override IEntity AddSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity DeleteSourceEntity(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }


        public override Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        public override IEntity UpdateSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }


    }
}
