using Haas.HR.Models;
using Microsoft.EntityFrameworkCore;

namespace Haas.HR
{
    /// <summary>
    /// Interface implemented by any HR Data Source that contributes data to a Haas employee
    /// </summary>
    public interface IHRDataSource
    {
        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<IHRDataSourceSynchronizeResult> SynchronizeEntityData(IHRDataSourceSynchronizeSettings settings);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity AddSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity DeleteSourceEntity(IHRDataSourceConnectionSettings settings, string ID);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity UpdateSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee);

        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHRDataSourceUploadResult UploadEntityData(IHRDataSourceUploadSettings settings);

        /// <summary>
        /// Downloads data from the HR Data Source into the HR Data Wharehouse
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task<IHRDataSourceDownloadResult> DownloadEntityData(IHRDataSourceDownloadSettings settings);

        /// <summary>
        /// Merges data from the Entity table associated with this Hr Data Source into the Entity Master record
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHRDataSourceMergeResult MergeEntityData(IHRDataSourceMergeSettings settings);

        /// <summary>
        /// Returns the URL to the profile view for the specified user id in the HR Data Source
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        string GetEntityProfileUrl(string uid);

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        IQueryable<IEntity> GetDestinationEntities();

        /// <summary>
        /// Adds the list of employees to the destination
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        int AddDestinationEntities(IList<IEntity> employees);

        /// <summary>
        /// Udpates the list of employees in the destination
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        //int UpdateDestinationEntities(IList<IEntity> employees);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity AddDestinationEntity(IEntity employee);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity DeleteDestinationEntity(IEntity employee);

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IEntity UpdateDestinationEntity(IEntity employee);
    }

    /// <summary>
    /// Contains the result of a data source upload
    /// </summary>
    public interface IHRDataSourceUploadResult
    {

    }

    /// <summary>
    /// Contains the result of a data source upload
    /// </summary>
    public interface IHRDataSourceSynchronizeResult
    {
        IHRDataSourceConnectionSettings? ConnectionSettings { get; set; }
        IHRDataSourceUploadResult? UploadResult { get; set; }
        IHRDataSourceDownloadResult? DownloadResult { get; set;  }
        IHRDataSourceMergeResult? MergeResult { get; set; }
    }

    /// <summary>
    /// Contains the settings to use as part of the data source upload
    /// </summary>
    public interface IHRDataSourceSynchronizeSettings
    {
        IHRDataSourceConnectionSettings ConnectionSettings { get; }
        IHRDataSourceUploadSettings UploadSettings { get; }
        IHRDataSourceDownloadSettings DownloadSettings { get; }
        IHRDataSourceMergeSettings MergeSettings { get; }
    }

    /// <summary>
    /// Contains the settings to use as part of the data source upload
    /// </summary>
    public interface IHRDataSourceUploadSettings
    {
        IHRDataSourceConnectionSettings ConnectionSettings { get; }
    }

    /// <summary>
    /// Contains the result of a data source download
    /// </summary>
    public interface IHRDataSourceDownloadResult
    {
        int RecordsDownloadedCount { get; set; }
    }

    /// <summary>
    /// Contains the settings to use as part of the download operation
    /// </summary>
    public interface IHRDataSourceDownloadSettings
    {
        IHRDataSourceConnectionSettings ConnectionSettings { get; }
      
    }

    /// <summary>
    /// Contains the result of a data source merge
    /// </summary>
    public interface IHRDataSourceMergeResult
    {
    }

    /// <summary>
    /// Contains the settings to use as part of the merge operation
    /// </summary>
    public interface IHRDataSourceMergeSettings
    {
        IHRDataSourceConnectionSettings ConnectionSettings { get; }
    }

    /// <summary>
    /// Contains the settings to use to connect to a HR Data Source
    /// </summary>
    public interface IHRDataSourceConnectionSettings
    {
        int? ID { get; set; }
        string? Name { get; set; }
        string? Desription { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
        string? URL { get; set; }
        string? TypeName { get; set; }
        int? ExecutionOrder { get; set; }
    }
}