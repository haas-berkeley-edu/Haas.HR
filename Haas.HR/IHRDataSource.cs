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
        IHRDataSourceSynchronizeResult SynchronizeEmployeeData(IHRDataSourceSynchronizeSettings settings);

        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHRDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings);

        /// <summary>
        /// Downloads data from the HR Data Source into the HR Data Wharehouse
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHRDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings);

        /// <summary>
        /// Merges data from the Employee table associated with this Hr Data Source into the Employee Master record
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHRDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings);

        /// <summary>
        /// Returns the URL to the profile view for the specified user id in the HR Data Source
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        string GetEmployeeProfileUrl(string uid);
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
        IHRDataSourceUploadResult UploadResult { get; }
        IHRDataSourceDownloadResult DownloadResult { get; }
        IHRDataSourceMergeResult MergeResult { get; }
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
        int ID { get; set; }
        string Name { get; set; }
        string Desription { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string URL { get; set; }
        string Type { get; set; }
        int ExecutionOrder { get; set; }
    }
}