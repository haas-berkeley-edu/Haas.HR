using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    /// <summary>
    /// Interface implemented by a model that is Entity
    /// </summary>
    public interface IEntity
    {
        string? PrimaryKey { get; set; }

        DateTime? CreatedOn { get; set; }
        DateTime? LastUpdatedOn { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
