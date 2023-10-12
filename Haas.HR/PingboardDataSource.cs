using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;

namespace Haas.HR
{
    /// <summary>
    /// Impelemntation of the Haas Pingboard instance.  This class is responsible for handling the downloading of the data in that instance
    /// into the Pingboard table in the HR Data Wharehouse.  It is also responsible for updating the Master employee record in the master employee
    /// table in the Data Wharehouse with the fields in Pingboard that are the master fields (ie. Reports To, Position Title).  Finally this class
    /// is responsible for uploading data from the Master employee record in the HR Data Wharehouse that are referenced in Pingboard (ie. Seat, Phone Number).
    /// The GetEmployeeProfile url method takes in the uid of the employee in question and returns the url that you would use to navigate to that employee
    /// record in Pingboard.
    /// </summary>
    public class PingboardDataSource : HRDataSourceBase
    {
        public override IHRDataSourceDownloadResult DownloadEmployeeData(IHRDataSourceDownloadSettings settings)
        {
            IHRDataSourceDownloadResult result = new HRDataSourceDownloadResult();
            //add logic to call Pingboard using connection settings

            //loop through results

            //find empmployee
            //HRDataSourceManager.HRDbContext.PingboardEmployees.Find()

            //if does not exist add it

            //if exists update it

            //loop through existing employees and see if they exist in this result set

            //if they don't then update the DeletedOn date field value

            return result;
        }

        public override string GetEmployeeProfileUrl(string uid)
        {
            throw new NotImplementedException();
        }

        public override IHRDataSourceUploadResult UploadEmployeeData(IHRDataSourceUploadSettings settings)
        {
            IHRDataSourceUploadResult result = new HRDataSourceUploadResult();
            //get curent employee records

            //get all master employee records

            //if the record does not exist in Pingboard then create it and determine if we can assign someone they report to
            //logic should take into account that some highe level leaders at Haas like Dean and Erika should not have anyone auto assigned to them
            //Add them also to the new staff or faculty group

            //if the record exists then update it with latest values along with alst updated date

            //if the master employee record is deleted the update the deleted field

            //get all of the pingboard recors in the cloud

            //loop through all of those records

            //if the record does not exist, then cfreate it

            //if the record exists, and the deletedOn field is null then update it

            //if the record is deleted then disable it in Pingboard

            return result;
        }

        public override IHRDataSourceMergeResult MergeEmployeeData(IHRDataSourceMergeSettings settings)
        {
            IHRDataSourceMergeResult result = new HRDataSourceMergeResult();

            //loop through the pingboard employee records
            foreach(PingboardEmployee pingboardEmployee in HRDataSourceManager.HRDbContext.PingboardEmployees)
            {
                //if the record exists in the master employee record then update it with the Working Title and Reports To
                MasterEmployee? masterEmployee = HRDataSourceManager.HRDbContext.MasterEmployees.Find(null);
                if (masterEmployee == null)
                {
                    continue;
                }
                //update the master employee record

                HRDataSourceManager.HRDbContext.MasterEmployees.Update(masterEmployee);
            }
            return result;
        }
    }
}
