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
        public HRDataSourceSynchronizeSettings(IHRDataSourceConnectionSettings connectionSettings)
        {
            this.connectionSettings = connectionSettings;
        }

        public IHRDataSourceConnectionSettings ConnectionSettings
        {
            get { return this.connectionSettings; }
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
}
