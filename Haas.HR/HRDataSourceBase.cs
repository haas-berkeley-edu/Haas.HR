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
    /// Base class use to store common functionality used by all HR Data Sources
    /// </summary>
    public abstract class HRDataSourceBase : IHRDataSource
    {
        public virtual IHRDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            IHRDataSourceDownloadResult result = new HRDataSourceDownloadResult();
            IList<IEmployee> sourceEmployees = this.GetSourceEmployees(settings.ConnectionSettings);
            IList<IEmployee> destinationEmployees = this.GetDestinationEmployees();
            foreach (IEmployee sourceEmployee in sourceEmployees)
            {
                //check to see if the master employee record exists, if it does not then do nothing since only UCPath can create
                //new master employee records
                IEmployee destinationEmployee = destinationEmployees.Single(a => a.UID == sourceEmployee.UID);
                if (destinationEmployee == null)
                {
                    //if record does not exist then add it
                    sourceEmployee.CreateOn = DateTime.Now;
                    this.AddDestinationEmployee(sourceEmployee);
                    continue;
                }
                //update the pingboard employee record with the values in pingboard
                sourceEmployee.CreateOn = destinationEmployee.CreateOn;
                sourceEmployee.DeletedOn = destinationEmployee.DeletedOn;
                sourceEmployee.LastUpdatedOn = DateTime.Now;
                this.AddDestinationEmployee(sourceEmployee);
            }
            return result;
        }

        public virtual string GetEmployeeProfileUrl(string uid)
        {
            throw new NotImplementedException();
        }

        public virtual IHRDataSourceSynchronizeResult SynchronizeEmployeeData(IHRDataSourceSynchronizeSettings settings)
        {
            IHRDataSourceSynchronizeResult result = new HRDataSourceSynchronizeResult();
            result.DownloadResult = this.DownloadEmployeeData(settings.DownloadSettings);
            if (result.DownloadResult == null)
            {
                return result;
            }
            result.MergeResult = this.MergeEmployeeData(settings.MergeSettings);
            if (result.MergeResult == null)
            {
                return result;
            }
            result.UploadResult = this.UploadEmployeeData(settings.UploadSettings);
            return result;
        }

        public virtual IHRDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            IHRDataSourceUploadResult result = new HRDataSourceUploadResult();
            IList<IEmployee> sourceEmployees = this.GetSourceEmployees(settings.ConnectionSettings);
            IList<IEmployee> destinationEmployees = this.GetDestinationEmployees();

            //loop through existing pingboard employees
            foreach (IEmployee destinationEmployee in destinationEmployees)
            {
                //should the employee be removed
                if (destinationEmployee.DeletedOn != null)
                {
                    this.DeleteSourceEmployee(settings.ConnectionSettings, destinationEmployee.ID);
                    continue;
                }

                //check to tsee if the source employee record exists
                IEmployee sourceEmployee = sourceEmployees.Single(a => a.ID == destinationEmployee.ID);
                if (sourceEmployee == null)
                {
                    //if record does not exist then add it
                    this.AddSourceEmployee(settings.ConnectionSettings, destinationEmployee);
                    continue;
                }

                //update the pingboard employee record with the values in pingboard
                this.UpdateSourceEmployee(settings.ConnectionSettings, destinationEmployee);
            }
            return result;
        }

        public virtual IHRDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            IHRDataSourceMergeResult result = new HRDataSourceMergeResult();

            //loop through the pingboard employee records
            IList<IEmployee> employees = this.GetDestinationEmployees();
            foreach (IEmployee employee in employees)
            {
                //if the record exists in the master employee record then update it with the Working Title and Reports To
                MasterEmployee? masterEmployee = HRDataSourceManager.HRDbContext.MasterEmployees.Single(a => a.UID == employee.UID);
                if ((masterEmployee == null) || (masterEmployee.DeletedOn != null))
                {
                    this.DeleteDestinationEmployee(employee);
                    continue;
                }

                //update the master employee record
                HRDataSourceManager.HRDbContext.MasterEmployees.Update(masterEmployee);
            }
            return result;
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IList<IEmployee> GetSourceEmployees(IHRDataSourceConnectionSettings settings);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEmployee AddSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEmployee DeleteSourceEmployee(IHRDataSourceConnectionSettings settings, string ID);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEmployee UpdateSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee);

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public abstract IList<IEmployee> GetDestinationEmployees();

        public abstract IEmployee AddDestinationEmployee(IEmployee employee);

        public abstract IEmployee DeleteDestinationEmployee(IEmployee employee);

        public abstract IEmployee UpdateDestinationEmployee(IEmployee employee);
    }

    public class HRDataSourceDownloadSettings : IHRDataSourceDownloadSettings
    {
        private IHRDataSourceConnectionSettings connectionSettings = null;
        public HRDataSourceDownloadSettings(IHRDataSourceConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings;
        }

        public IHRDataSourceConnectionSettings ConnectionSettings
        {
            get { return this.connectionSettings; }
        }
    }

    public class HRDataSourceSynchronizeSettings : IHRDataSourceSynchronizeSettings
    {
        private IHRDataSourceConnectionSettings connectionSettings = null;
        private IHRDataSourceMergeSettings mergeSettings = null;
        private IHRDataSourceUploadSettings uploadSettings = null;
        private IHRDataSourceDownloadSettings downloadSettings = null;

        public HRDataSourceSynchronizeSettings(IHRDataSourceConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings;
        }

        public IHRDataSourceConnectionSettings ConnectionSettings
        {
            get { return this.connectionSettings; }
        }

        public IHRDataSourceUploadSettings UploadSettings
        {
            get { return this.uploadSettings; }
        }

        public IHRDataSourceDownloadSettings DownloadSettings
        {
            get { return this.downloadSettings; }
        }

        public IHRDataSourceMergeSettings MergeSettings
        {
            get { return this.mergeSettings; }
        }
    }

    public class HRDataSourceUploadSettings : IHRDataSourceUploadSettings
    {
        private IHRDataSourceConnectionSettings connectionSettings = null;
        public HRDataSourceUploadSettings(IHRDataSourceConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings;
        }

        public IHRDataSourceConnectionSettings ConnectionSettings
        {
            get { return this.connectionSettings; }
        }
    }

    public class HRDataSourceMergeSettings : IHRDataSourceMergeSettings
    {
        private IHRDataSourceConnectionSettings connectionSettings = null;
        public HRDataSourceMergeSettings(IHRDataSourceConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings;
        }

        public IHRDataSourceConnectionSettings ConnectionSettings
        {
            get { return this.connectionSettings; }
        }
    }

    public class HRDataSourceUploadResult : IHRDataSourceUploadResult
    {

    }

    public class HRDataSourceDownloadResult : IHRDataSourceDownloadResult
    {

    }

    public class HRDataSourceMergeResult : IHRDataSourceMergeResult
    {

    }

    public class HRDataSourceSynchronizeResult : IHRDataSourceSynchronizeResult
    {
        public IHRDataSourceConnectionSettings? ConnectionSettings
        {
            get;
            set;
        }

        public IHRDataSourceUploadResult? UploadResult
        {
            get;
            set;
        }

        public IHRDataSourceDownloadResult? DownloadResult
        {
            get;
            set;
        }
        public IHRDataSourceMergeResult? MergeResult
    {
            get;
            set;
        }
    }
}
