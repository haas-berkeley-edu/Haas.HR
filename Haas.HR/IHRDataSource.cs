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
        IHrDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings);

        /// <summary>
        /// Downloads data from the HR Data Source into the HR Data Wharehouse
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        IHrDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings);
 
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
    public interface IHrDataSourceUploadResult
    {

    }

    /// <summary>
    /// Contains the settings to use as part of the data source upload
    /// </summary>
    public interface IHRDataSourceUploadSettings
    {

    }

    /// <summary>
    /// Contains the result of a data source download
    /// </summary>
    public interface IHrDataSourceDownloadResult
    {
    }

    /// <summary>
    /// Contains the settings to use as part of the download operation
    /// </summary>
    public interface IHRDataSourceDownloadSettings
    {

    }
}