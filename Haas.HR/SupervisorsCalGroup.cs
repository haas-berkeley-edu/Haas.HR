using Haas.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    /// <summary>
    /// Implementation of the integratio between calgroups and HR Data Warehouse
    /// </summary>
    public class SupervisorsCalGroup : CalGroupBase
    {

        /// <summary>
        /// Synchornizes the membership of the Haas Supervisors table with CalGroups
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected override void OnAfterMergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            return;
        }

    }
}
