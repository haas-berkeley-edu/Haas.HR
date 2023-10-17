﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;

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

            //loop through the datasource source employee records
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
            this.OnAfterMergeEmployeeData(settings);
            return result;
        }

        /// <summary>
        /// Allows extending classes to implement their own merge logic to be executed after the merge operation
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected virtual void OnAfterMergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            return;
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

        //Returns the API Credentials from the rsp_API_Resources_S table in PDB01 Database
        protected ApiCredentials GetApiCredentials(string apiName)
        {
            ApiCredentials result = new ApiCredentials();
            using (SqlConnection sqlConn  = new SqlConnection("Server=sql-pdb01.haas.berkeley.edu;Database=Reference;Integrated Security=True")) { 
                sqlConn.Open();

                using (SqlCommand sqlCmd = sqlConn.CreateCommand())
                {
                    sqlCmd.CommandText = "rsp_API_Resources_S";
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCmd.Parameters.Add("@API_Name", System.Data.SqlDbType.VarChar).Value = apiName;

                    SqlParameter sqlOutputParam1 = sqlCmd.Parameters.Add("@API_ID", System.Data.SqlDbType.VarChar);
                    sqlOutputParam1.Direction = System.Data.ParameterDirection.Output;
                    sqlOutputParam1.Size = 500;

                    SqlParameter sqlOutputParam2 = sqlCmd.Parameters.Add("@API_Key", System.Data.SqlDbType.VarChar);
                    sqlOutputParam2.Direction = System.Data.ParameterDirection.Output;
                    sqlOutputParam2.Size = 500;

                    try { sqlCmd.ExecuteNonQuery(); }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    result.APIID = (string)sqlCmd.Parameters["@API_ID"].Value;
                    result.APIKey = (string)sqlCmd.Parameters["@API_Key"].Value;
                }
            }

            return result;
        }
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
