using Haas.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    /// <summary>
    /// Base class use to store common functionality used by all HR Data Sources
    /// </summary>
    public abstract class HRDataSourceBase : IHRDataSource
    {
        public virtual IHRDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            throw new NotImplementedException();
        }

        public virtual string GetEmployeeProfileUrl(string uid)
        {
            throw new NotImplementedException();
        }

        public virtual IHRDataSourceSynchronizeResult SynchronizeEmployeeData(IHRDataSourceSynchronizeSettings settings)
        {
            throw new NotImplementedException();
        }

        public virtual IHRDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            throw new NotImplementedException();
        }

        public virtual IHRDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual List<IEmployee> GetSourceEmployees(IHRDataSourceConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEmployee AddSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEmployee DeleteSourceEmployee(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the employee data for the specified settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEmployee UpdateSourceEmployee(IHRDataSourceConnectionSettings settings, IEmployee employee)
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
