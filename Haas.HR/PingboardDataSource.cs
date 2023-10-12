using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;

namespace Haas.HR
{
    /// <summary>
    /// Impelemntation of the Haas Pingboard instance.  This class is responsible for handling the downloading of the data in that instance
    /// into the Pingboard table in the HR Data Wharehouse.  It is also responsible for updating the Master employee record in the master employee
    /// table in the Data Wharehouse with the fields in Pingboard that are the master fields (ie. Reports To, Position Title).  Finally this class
    /// is responsible for uploading data from the Master employee record in the HR Data Wharehouse that are referenced in Pingboard (ie. Seat, Phone Number).
    /// The GetEmployeeProfile url method takes in the uid of the employee in question and returns the url that you would use to navigate to that employee
    /// record in Pingboard.
    /// </summary>
    public class PingboardDataSource : HRDataSourceBase
    {
        public override string GetEmployeeProfileUrl(string uid)
        {
            PingboardEmployee pingboardEmployee = HRDataSourceManager.HRDbContext.PingboardEmployees.Single(a => a.UID == uid);
            if (pingboardEmployee == null)
            {
                return null;
            }
            return "https://orgchart.haas.berkele.edu/id=" + pingboardEmployee.ID;
        }

        public override IEmployee AddDestinationEmployee(IEmployee employee)
        {
            PingboardEmployee pingboardEmployee = employee as PingboardEmployee;
            if (pingboardEmployee == null)
            {
                return null;
            }
            pingboardEmployee.CreateOn = DateTime.Now;
            HRDataSourceManager.HRDbContext.PingboardEmployees.Add(pingboardEmployee);
            return employee;
        }

        public override IEmployee DeleteDestinationEmployee(IEmployee employee)
        {
            PingboardEmployee pingboardEmployee = employee as PingboardEmployee;
            if (pingboardEmployee == null)
            {
                return null;
            }
            pingboardEmployee.DeletedOn = DateTime.Now;
            HRDataSourceManager.HRDbContext.PingboardEmployees.Update(pingboardEmployee);
            return employee;
        }

        public override IEmployee UpdateDestinationEmployee(IEmployee employee)
        {
            PingboardEmployee pingboardEmployee = employee as PingboardEmployee;
            if (pingboardEmployee == null)
            {
                return null;
            }
            pingboardEmployee.LastUpdatedOn = DateTime.Now;
            HRDataSourceManager.HRDbContext.PingboardEmployees.Update(pingboardEmployee);
            return employee;
        }

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public override IList<IEmployee> GetDestinationEmployees()
        {
            return HRDataSourceManager.HRDbContext.PingboardEmployees.ToList<IEmployee>();
        }

        public override IList<IEmployee> GetSourceEmployees(IHRDataSourceConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        public override IEmployee AddSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
        {
            throw new NotImplementedException();
        }

        public override IEmployee DeleteSourceEmployee(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }

        public override IEmployee UpdateSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
        {
            throw new NotImplementedException();
        }
    }
}
