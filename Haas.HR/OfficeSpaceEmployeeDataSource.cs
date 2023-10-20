using Haas.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Azure.Core;

namespace Haas.HR
{
    public class OfficeSpaceEmployeeDataSource : HRDataSourceBase
    {

        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public override IQueryable<IEntity> GetDestinationEntities()
        {
            return HRDataSourceManager.HRDbContext.OfficeSpaceEmployees;
        }

        /// <summary>
        /// Returns a complete list of all of the users in the Pingboard database
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public override async Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings)
        {
            List<OfficeSpaceEmployee> result = new List<OfficeSpaceEmployee>();
            ApiCredentials apiCredentials = this.GetApiCredentials("OFFICESPACE_LEGACY");
            string officeSpaceAccessToken = apiCredentials.APIKey;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + officeSpaceAccessToken);
                string officeSpaceEmployeesApiUrl = "https://haas.officespacesoftware.com/api/1/employees";
                var resultType = new
                {
                    response = new List<OfficeSpaceEmployee>()
                }.GetType();
                var responseMessage = await httpClient.GetAsync(officeSpaceEmployeesApiUrl);
                Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
                dynamic? responseMessageContent = await responseMessage.Content.ReadFromJsonAsync(resultType);
                List<OfficeSpaceEmployee> nextOfficeSpaceEmployees = responseMessageContent.response;
                result.AddRange(nextOfficeSpaceEmployees);
            }
            return result.Cast<IEntity>().ToList();
        }

        public override IEntity AddSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity DeleteSourceEntity(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }

        public override IEntity UpdateSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override int AddDestinationEntities(IList<IEntity> employees)
        {
            HRDataSourceManager.HRDbContext.OfficeSpaceEmployees.AddRange(employees.Cast<OfficeSpaceEmployee>().ToList());
            return employees.Count;
        }

        /*
        public override int UpdateDestinationEntities(IList<IEntity> employees)
        {
            HRDataSourceManager.HRDbContext.OfficeSpaceEmployees.UpdateRange(employees.Cast<OfficeSpaceEmployee>().ToList());
            return employees.Count;
        }
        */
    }
}
