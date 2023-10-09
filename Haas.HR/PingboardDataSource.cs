using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    public class PingboardDataSource : HRDataSourceBase
    {
        public override IHrDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            throw new NotImplementedException();
        }

        public override string GetEmployeeProfileUrl(string uid)
        {
            throw new NotImplementedException();
        }

        public override IHrDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
