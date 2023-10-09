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
        public virtual IHrDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            throw new NotImplementedException();
        }

        public virtual string GetEmployeeProfileUrl(string uid)
        {
            throw new NotImplementedException();
        }

        public virtual IHrDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            throw new NotImplementedException();
        }

        public virtual IHrDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
