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
        /*
        public override IHRDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            IHRDataSourceDownloadResult result = new HRDataSourceDownloadResult();
            IList<IEmployee> pingboardEmployees = this.GetSourceEmployees(settings.ConnectionSettings);
            foreach(IEmployee employee in pingboardEmployees)
            {
                //check to see if the master employee record exists, if it does not then do nothing since only UCPath can create
                //new master employee records
                PingboardEmployee pingboardEmployee = (PingboardEmployee)employee;
                PingboardEmployee existingPingboardEmployee = HRDataSourceManager.HRDbContext.PingboardEmployees.Single(a => a.UID == pingboardEmployee.UID);
                if (existingPingboardEmployee == null)
                {
                    //if record does not exist then add it
                    pingboardEmployee.CreateOn = DateTime.Now;
                    HRDataSourceManager.HRDbContext.PingboardEmployees.Add(pingboardEmployee);
                    continue;
                }
                //update the pingboard employee record with the values in pingboard
                pingboardEmployee.CreateOn = existingPingboardEmployee.CreateOn;
                pingboardEmployee.DeletedOn = existingPingboardEmployee.DeletedOn;
                pingboardEmployee.LastUpdatedOn = DateTime.Now;
                HRDataSourceManager.HRDbContext.PingboardEmployees.Update(pingboardEmployee);
            }
            return result;
        }
        */

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
            HRDataSourceManager.HRDbContext.PingboardEmployees.Add(employee as PingboardEmployee);
            return employee;
        }

        public override IEmployee DeleteDestinationEmployee(IEmployee employee)
        {
            employee.DeletedOn = DateTime.Now;
            HRDataSourceManager.HRDbContext.PingboardEmployees.Update(employee as PingboardEmployee);
            return employee;
        }

        public override IEmployee UpdateDestinationEmployee(IEmployee employee)
        {
            HRDataSourceManager.HRDbContext.PingboardEmployees.Update(employee as PingboardEmployee);
            return employee;
    }

        /*
        public override IHRDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            IHRDataSourceUploadResult result = new HRDataSourceUploadResult();
            IList<IEmployee> cloudPingboardEmployees = this.GetSourceEmployees(settings.ConnectionSettings);

            //loop through existing pingboard employees
            foreach (PingboardEmployee pingboardEmployee in HRDataSourceManager.HRDbContext.PingboardEmployees)
            {
                //check to see if the master employee record exists, if it does not then do nothing since only UCPath can create
                //new master employee records
                IEmployee cloudEmployee = cloudPingboardEmployees.Single(a => a.ID == pingboardEmployee.ID);
                PingboardEmployee cloudPingboardEmployee = (PingboardEmployee)cloudEmployee;
                if (cloudPingboardEmployee == null)
                {
                    //if record does not exist then add it
                    this.AddSourceEmployee(settings.ConnectionSettings, pingboardEmployee);
                    continue;
                }
                //update the pingboard employee record with the values in pingboard
                this.UpdateSourceEmployee(settings.ConnectionSettings, pingboardEmployee);
            }

            //loop through the cloud pingboard employees
            foreach(IEmployee cloudEmployee in cloudPingboardEmployees)
            {
                PingboardEmployee cloudPingboardEmployee = (PingboardEmployee)cloudEmployee;
                PingboardEmployee existingPingboardEmployee = HRDataSourceManager.HRDbContext.PingboardEmployees.Single(a => a.ID == cloudEmployee.ID);
                if (existingPingboardEmployee.DeletedOn != null)
                {
                    continue;
                }
                this.DeleteSourceEmployee(settings.ConnectionSettings, cloudEmployee.ID);
            }
            return result;
        }
        */

        /*
        public override IHRDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            IHRDataSourceMergeResult result = new HRDataSourceMergeResult();

            //loop through the pingboard employee records
            foreach(PingboardEmployee pingboardEmployee in HRDataSourceManager.HRDbContext.PingboardEmployees)
            {
                //if the record exists in the master employee record then update it with the Working Title and Reports To
                MasterEmployee? masterEmployee = HRDataSourceManager.HRDbContext.MasterEmployees.Single(a => a.ID == pingboardEmployee.ID);
                if (masterEmployee == null)
                {
                    continue;
                }
                //update the master employee record

                HRDataSourceManager.HRDbContext.MasterEmployees.Update(masterEmployee);
            }
            return result;
        }
        */

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public override IList<IEmployee> GetDestinationEmployees()
        {
            return HRDataSourceManager.HRDbContext.PingboardEmployees.ToList<IEmployee>();
        }
    }
}
