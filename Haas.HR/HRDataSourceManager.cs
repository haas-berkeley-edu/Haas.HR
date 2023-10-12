using Haas.HR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    public class HRDataSourceManager
    {
        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceSynchronizeResult> SynchronizeEmployeeData(IHRDataSourceSynchronizeSettings settings)
        {
            HRDbContext context = new HRDbContext(null);
            List<IHRDataSourceSynchronizeResult> results = new List<IHRDataSourceSynchronizeResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.Type);
                if (hrDataSource == null)
                {
                    continue;
                }
                IHRDataSourceUploadSettings UploadSettings = new HRDataSourceUploadSettings(connectionSettings);
                IHRDataSourceUploadResult result = hrDataSource.UploadEmployeeData(UploadSettings);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceUploadResult> UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            HRDbContext context = new HRDbContext(null);
            List<IHRDataSourceUploadResult> results = new List<IHRDataSourceUploadResult>();
            foreach(IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.Type);
                if (hrDataSource == null)
                {
                    continue;
                }
                IHRDataSourceUploadSettings uploadSettings = new HRDataSourceUploadSettings(connectionSettings);
                IHRDataSourceUploadResult result = hrDataSource.UploadEmployeeData(uploadSettings);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// Downloads data from the HR Data Source into the HR Data Wharehouse
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceDownloadResult> DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            HRDbContext context = new HRDbContext(null);
            List<IHRDataSourceDownloadResult> results = new List<IHRDataSourceDownloadResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.Type);
                if (hrDataSource == null)
                {
                    continue;
                }
                HRDataSourceDownloadSettings downloadSettings = new HRDataSourceDownloadSettings(connectionSettings);
                IHRDataSourceDownloadResult result = hrDataSource.DownloadEmployeeData(downloadSettings);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// Merges data from the Employee table associated with this Hr Data Source into the Employee Master record
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceMergeResult> MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            HRDbContext context = new HRDbContext(null);
            List<IHRDataSourceMergeResult> results = new List<IHRDataSourceMergeResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.Type);
                if (hrDataSource == null)
                {
                    continue;
                }
                HRDataSourceMergeSettings mergeSettings = new HRDataSourceMergeSettings(connectionSettings);
                IHRDataSourceMergeResult result = hrDataSource.MergeEmployeeData(mergeSettings);
                results.Add(result);
            }
            return results;
        }

        private IHRDataSource? GetHRDataSource(string typeName)
        {
            Type? type = Type.GetType(typeName);
            if (type == null)
            {
                return null;
            }
            return (IHRDataSource?)System.Activator.CreateInstance(type);
        }

    }
}
