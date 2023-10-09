using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    /// <summary>
    /// Impelemntation of the Haas Pingboard instance.  This class is responsible for handling the downloading of the data in that instance
    /// into the Pingboard table in the HR Data Wharehouse.  It is also responsible for updating the Master employee record in the master employee
    /// table in the Data Wharehouse with the fields in Pingboard that are the master fields (ie. Reports To, Position Title).  Finally this class
    /// is responsible for uploading data from the Master employee record in the HR Data Wharehouse that are referenced in Pingboard (ie. Seat, Phone Number)
    /// </summary>
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
