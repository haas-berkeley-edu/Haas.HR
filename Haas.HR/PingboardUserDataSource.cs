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

namespace Haas.HR
{
    public class PingboardUserDataSource : PingboardDataSourceBase
    {
        public override string GetEntityProfileUrl(string uid)
        {
            PingboardUser pingboardUser = HRDataSourceManager.HRDbContext.PingboardUsers.Single(a => a.UID == uid);
            if (pingboardUser == null)
            {
                return null;
            }
            return "https://orgchart.haas.berkele.edu/id=" + pingboardUser.id;
        }


        /// <summary>
        /// Returns the DbSet of employees from the databasefor this HRDataSource 
        /// </summary>
        /// <returns></returns>
        public override IQueryable<IEntity> GetDestinationEntities()
        {
            return HRDataSourceManager.HRDbContext.PingboardUsers;
        }

        /// <summary>
        /// Returns a complete list of all of the users in the Pingboard database
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public override async Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings)
        {
            List<PingboardUser> result = new List<PingboardUser>();
            using (HttpClient httpClient = new HttpClient())
            {
                string pingboardAccessToken = await this.GetPingboardAccessToken();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + pingboardAccessToken);
                string pingboardUsersApiUrl = "https://app.pingboard.com/api/v2/users?page_size=100&page=1";
                var resultType = new
                {
                    users = new List<PingboardUser>(),
                    meta = new
                    {
                        users = new
                        {
                            next_href = ""
                        }
                    },
                }.GetType();
                while (true)
                {
                    var responseMessage = await httpClient.GetAsync(pingboardUsersApiUrl);
                    dynamic? responseMessageContent = await responseMessage.Content.ReadFromJsonAsync(resultType);
                    List<PingboardUser> nextPingboardUsers = responseMessageContent.users;
                    result.AddRange(nextPingboardUsers);
                    if (String.IsNullOrWhiteSpace(responseMessageContent.meta.users.next_href))
                    {
                        break;
                    }
                    pingboardUsersApiUrl = "https://app.pingboard.com" + responseMessageContent.meta.users.next_href;
                }
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
            HRDataSourceManager.HRDbContext.PingboardUsers.AddRange(employees.Cast<PingboardUser>().ToList());
            return employees.Count;
        }

        /*
        public override int UpdateDestinationEntities(IList<IEntity> employees)
        {
            HRDataSourceManager.HRDbContext.PingboardUsers.UpdateRange(employees.Cast<PingboardUser>().ToList());
            return employees.Count;
        }
        */
    }
}
