using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Haas.HR.Models;
using System.Net.Http.Json;

namespace Haas.HR
{
    /// <summary>
    /// Base class use to store common functionality used by all HR Data Sources
    /// </summary>
    public abstract class HRDataSourceBase : IHRDataSource
    {
        //private static HttpClient httpClient = null;

        static HRDataSourceBase()
        {
            //httpClient = new HttpClient();
        }

        public virtual async Task<IHRDataSourceDownloadResult> DownloadEntityData(IHRDataSourceDownloadSettings settings)
        {
            IHRDataSourceDownloadResult result = new HRDataSourceDownloadResult();
            IList<IEntity> sourceEntities = await this.GetSourceEntities(settings.ConnectionSettings);
            IQueryable<IEntity> destinationEntities = this.GetDestinationEntities();
            IList<IEntity> addEntities = (IList<IEntity>)this.GetDbSetInserts(sourceEntities, destinationEntities);
            this.AddDestinationEntities(addEntities);
            IList<IEntity> updateEntities = (IList<IEntity>)this.GetDbSetUpdates(sourceEntities, destinationEntities);
            this.UpdateDestinationEntities(updateEntities);
            HRDataSourceManager.HRDbContext.SaveChanges();
            return result;
        }

        /*
        public static HttpClient HttpClient 
        {
            get { return httpClient; }
        }
        */

        public virtual string GetEntityProfileUrl(string uid)
        {
            return null;
        }

        public virtual async Task<IHRDataSourceSynchronizeResult> SynchronizeEntityData(IHRDataSourceSynchronizeSettings settings)
        {
            IHRDataSourceSynchronizeResult result = new HRDataSourceSynchronizeResult();
            result.DownloadResult = await this.DownloadEntityData(settings.DownloadSettings);
            if (result.DownloadResult == null)
            {
                return result;
            }
            result.MergeResult = this.MergeEntityData(settings.MergeSettings);
            if (result.MergeResult == null)
            {
                return result;
            }
            result.UploadResult = this.UploadEntityData(settings.UploadSettings);
            return result;
        }

        public virtual IHRDataSourceUploadResult UploadEntityData(IHRDataSourceUploadSettings settings)
        {
            IHRDataSourceUploadResult result = new HRDataSourceUploadResult();
            /*
            IList<IEntity> sourceEntities = this.GetSourceEntities(settings.ConnectionSettings);
            IQueryable<IEntity> destinationEntities = this.GetDestinationEntities();

            //loop through existing pingboard employees
            foreach (IEntity destinationEntity in destinationEntities)
            {
                //should the employee be removed
                if (destinationEntity.DeletedOn != null)
                {
                    this.DeleteSourceEntity(settings.ConnectionSettings, destinationEntity.ID);
                    continue;
                }

                //check to tsee if the source employee record exists
                IEntity sourceEntity = sourceEntities.Single(a => a.ID == destinationEntity.ID);
                if (sourceEntity == null)
                {
                    //if record does not exist then add it
                    this.AddSourceEntity(settings.ConnectionSettings, destinationEntity);
                    continue;
                }

                //update the pingboard employee record with the values in pingboard
                this.UpdateSourceEntity(settings.ConnectionSettings, destinationEntity);
            }
            */
            return result;
        }

        /// <summary>
        /// Default implementation of Merging Entity data with the employee table
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual IHRDataSourceMergeResult MergeEntityData(IHRDataSourceMergeSettings settings)
        {
            IHRDataSourceMergeResult result = new HRDataSourceMergeResult();

            /*
            //loop through the datasource source employee records
            IQueryable<IEntity> employees = this.GetDestinationEntities();
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
            */
            this.OnAfterMergeEntityData(settings);
            return result;
        }

        /// <summary>
        /// Allows extending classes to implement their own merge logic to be executed after the merge operation
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected virtual void OnAfterMergeEntityData(IHRDataSourceMergeSettings settings)
        {
            return;
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEntity AddSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEntity DeleteSourceEntity(IHRDataSourceConnectionSettings settings, string ID);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public abstract IEntity UpdateSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee);

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<IEntity> GetDestinationEntities();

        public abstract IEntity AddDestinationEntity(IEntity employee);

        public abstract IEntity DeleteDestinationEntity(IEntity employee);

        public abstract IEntity UpdateDestinationEntity(IEntity employee);

        /*
        /// <summary>
        /// Helper method that returns a serialized JSON object based on a response
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<dynamic?> ReadFromJsonAsync(string url, Type type)
        {
            var response = await PingboardDataSource.HttpClient.GetAsync(url);
            dynamic? result = await response.Content.ReadFromJsonAsync(type);
            return result;
        }
        */

        /// <summary>
        /// Gives a collection of records to be merged into a destination collection, returns the records to add to the destination collection
        /// </summary>
        /// <param name="sourceCollection"></param>
        /// <param name="destinationCollection"></param>
        /// <returns></returns>
        protected IList<IEntity> GetDbSetInserts(IEnumerable<Haas.HR.Models.IEntity>? sourceCollection, IQueryable<IEntity> destinationCollection)
        {
            List<IEntity> result = new List<IEntity>();
            if (sourceCollection == null)
            {
                return result;
            }

            //loop through and add or update entities
            foreach (IEntity sourceEntity in sourceCollection)
            {
                dynamic? destinationEntity = destinationCollection.First<IEntity>(s => s.PrimaryKey == sourceEntity.PrimaryKey);
                if (destinationEntity == null)
                {
                    sourceEntity.CreatedOn = DateTime.Now;
                    sourceEntity.LastUpdatedOn = DateTime.Now;
                    sourceEntity.DeletedOn = null;
                    result.Add(sourceEntity);
                }
            }

            return result;
        }

        /// <summary>
        /// Gives a collection of records to be merged into a destination collection, returns the records to update in the destination collection
        /// </summary>
        /// <param name="sourceCollection"></param>
        /// <param name="destinationCollection"></param>
        /// <returns></returns>
        protected IList<IEntity> GetDbSetUpdates(IEnumerable<Haas.HR.Models.IEntity>? sourceCollection, IQueryable<IEntity> destinationCollection)
        {
            List<IEntity> result = new List<IEntity>();
            if (sourceCollection == null)
            {
                return result;
            }

            //loop through and add or update entities
            //loop through and add or update entities
            foreach (IEntity sourceEntity in sourceCollection)
            {
                dynamic? destinationEntity = destinationCollection.First<IEntity>(s => s.PrimaryKey == sourceEntity.PrimaryKey);
                if (destinationEntity != null)
                {
                    sourceEntity.LastUpdatedOn = DateTime.Now;
                    sourceEntity.DeletedOn = null;
                    result.Add(sourceEntity);
                }
            }

            //loop through and remove any deleteable entities
            foreach (IEntity destinationEntity in destinationCollection)
            {
                IEntity? sourceEntity = sourceCollection.First<IEntity>(s => s.PrimaryKey == destinationEntity.PrimaryKey);
                if (sourceEntity != null)
                {
                    continue;
                }

                destinationEntity.DeletedOn = DateTime.Now;
                result.Add(destinationEntity);
            }

            return result;
        }

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

        public int AddDestinationEntities(IList<IEntity> employee)
        {
            throw new NotImplementedException();
        }

        public int UpdateDestinationEntities(IList<IEntity> employee)
        {
            throw new NotImplementedException();
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
