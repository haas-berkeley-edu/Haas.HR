using Haas.HR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR
{
    public class PingboardGroupDataSource : PingboardDataSourceBase
    {
        public override IEntity AddDestinationEntity(IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity AddSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity DeleteDestinationEntity(IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity DeleteSourceEntity(IHRDataSourceConnectionSettings settings, string ID)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<IEntity> GetDestinationEntities()
        {
            return HRDataSourceManager.HRDbContext.PingboardGroups;
        }

        /// <summary>
        /// Returns a complete list of all of the users in the Pingboard database
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public override async Task<IList<IEntity>> GetSourceEntities(IHRDataSourceConnectionSettings settings)
        {
            List<PingboardGroup> result = new List<PingboardGroup>();
            using (HttpClient httpClient = new HttpClient())
            {
                string pingboardAccessToken = await this.GetPingboardAccessToken();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + pingboardAccessToken);
                string pingboardGroupsApiUrl = "https://app.pingboard.com/api/v2/groups?page_size=100&page=1";
                var resultType = new
                {
                    groups = new List<PingboardGroup>(),
                    meta = new
                    {
                        groups = new
                        {
                            next_href = ""
                        }
                    },
                }.GetType();
                while (true)
                {
                    var responseMessage = await httpClient.GetAsync(pingboardGroupsApiUrl);
                    dynamic? responseMessageContent = await responseMessage.Content.ReadFromJsonAsync(resultType);
                    List<PingboardGroup> nextPingboardGroups = responseMessageContent.groups;
                    result.AddRange(nextPingboardGroups);
                    if (String.IsNullOrWhiteSpace(responseMessageContent.meta.groups.next_href))
                    {
                        break;
                    }
                    pingboardGroupsApiUrl = "https://app.pingboard.com" + responseMessageContent.meta.groups.next_href;
                }
            }
            return result.Cast<IEntity>().ToList();
        }

        public override IEntity UpdateDestinationEntity(IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override IEntity UpdateSourceEntity(IHRDataSourceConnectionSettings settings, IEntity employee)
        {
            throw new NotImplementedException();
        }

        public override int AddDestinationEntities(IList<IEntity> employees)
        {
            HRDataSourceManager.HRDbContext.PingboardGroups.AddRange(employees.Cast<PingboardGroup>().ToList());
            return employees.Count;
        }

        /*
        public override int UpdateDestinationEntities(IList<IEntity> employees)
        {
            HRDataSourceManager.HRDbContext.PingboardGroups.UpdateRange(employees.Cast<PingboardGroup>().ToList());
            return employees.Count;
        }
        */
    }
}
