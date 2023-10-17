using Haas.HR.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    public class HRDataSourceManager
    {
        private static HRDbContext context;

        static HRDataSourceManager() {
            DbContextOptions<HRDbContext> options = new DbContextOptions<HRDbContext>();
            context = new HRDbContext(options);
            context.Database.EnsureCreated();
        }

        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceSynchronizeResult> SynchronizeAllEmployeeData(IHRDataSourceSynchronizeSettings settings)
        {
            List<IHRDataSourceSynchronizeResult> results = new List<IHRDataSourceSynchronizeResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings.OrderBy(a => a.ExecutionOrder))
            {
                HRDataSourceSynchronizeResult result = new HRDataSourceSynchronizeResult();
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.TypeName);
                if (hrDataSource == null)
                {
                    continue;
                }
                HRDataSourceDownloadSettings downloadSettings = new HRDataSourceDownloadSettings(connectionSettings);
                result.DownloadResult = hrDataSource.DownloadEmployeeData(downloadSettings);
                results.Add(result);
            }
            foreach (IHRDataSourceSynchronizeResult result in results)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(result.ConnectionSettings.TypeName);
                if (hrDataSource == null)
                {
                    continue;
                }
                HRDataSourceMergeSettings mergeSettings = new HRDataSourceMergeSettings(result.ConnectionSettings);
                result.MergeResult = hrDataSource.MergeEmployeeData(mergeSettings);
                results.Add(result);
            }
            foreach (IHRDataSourceSynchronizeResult result in results)
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(result.ConnectionSettings.TypeName);
                if (hrDataSource == null)
                {
                    continue;
                }
                HRDataSourceUploadSettings mergeSettings = new HRDataSourceUploadSettings(result.ConnectionSettings);
                result.UploadResult = hrDataSource.UploadEmployeeData(mergeSettings);
                results.Add(result);
            }
            return results;
        }

        /// <summary>
        /// Uploads data from the HR Data Wharehouse into HR Data Source
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public List<IHRDataSourceUploadResult> UploadAllEmployeeData(IHRDataSourceUploadSettings settings)
        {
            List<IHRDataSourceUploadResult> results = new List<IHRDataSourceUploadResult>();
            foreach(IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings.OrderBy(a => a.ExecutionOrder))
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.TypeName);
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
        public List<IHRDataSourceDownloadResult> DownloadAllEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            List<IHRDataSourceDownloadResult> results = new List<IHRDataSourceDownloadResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings.OrderBy(a => a.ExecutionOrder))
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.TypeName);
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
        public List<IHRDataSourceMergeResult> MergeAllEmployeeData(IHRDataSourceMergeSettings settings)
        {
            List<IHRDataSourceMergeResult> results = new List<IHRDataSourceMergeResult>();
            foreach (IHRDataSourceConnectionSettings connectionSettings in context.HRDataSourceConnectionSettings.OrderBy(a => a.ExecutionOrder))
            {
                IHRDataSource? hrDataSource = this.GetHRDataSource(connectionSettings.TypeName);
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

        public static HRDbContext HRDbContext
        {
            get { return context; }
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
