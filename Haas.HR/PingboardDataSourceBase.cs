using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;
using System.Net.Http.Json;
using System.Net.Http;

namespace Haas.HR
{
    /// <summary>
    /// Impelemntation of the Haas Pingboard instance.  This class is responsible for handling the downloading of the data in that instance
    /// into the Pingboard table in the HR Data Wharehouse.  It is also responsible for updating the Master employee record in the master employee
    /// table in the Data Wharehouse with the fields in Pingboard that are the master fields (ie. Reports To, Position Title).  Finally this class
    /// is responsible for uploading data from the Master employee record in the HR Data Wharehouse that are referenced in Pingboard (ie. Seat, Phone Number).
    /// The GetEntityProfile url method takes in the uid of the employee in question and returns the url that you would use to navigate to that employee
    /// record in Pingboard.
    /// </summary>
    public abstract class PingboardDataSourceBase : HRDataSourceBase
    {
        /// <summary>
        /// Returns an Access Token to use when working with the Pingboard API
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetPingboardAccessToken()
        {
            ApiCredentials apiCredentials = this.GetApiCredentials("PINGBOARD");
            using (HttpClient httpClient = new HttpClient())
            {
                string pingboardTokenApiUrl = "https://app.pingboard.com/oauth/token?grant_type=client_credentials";
                Dictionary<string, string> pingboardTokenPostParams = new Dictionary<string, string>();
                pingboardTokenPostParams["client_id"] = apiCredentials.APIID;
                pingboardTokenPostParams["client_secret"] = apiCredentials.APIKey;
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, pingboardTokenApiUrl) { Content = new FormUrlEncodedContent(pingboardTokenPostParams) };
                var resultType = new
                {
                    access_token = ""
                }.GetType(); 
                HttpResponseMessage res = await httpClient.SendAsync(req);
                dynamic? result = await res.Content.ReadFromJsonAsync(resultType);
                return result.access_token;
            }
        }


        /*
        public override async IList<IEntity> GetSourceEntities(IHRDataSourceConnectionSettings settings)
        {
            List<PingboardEntity> pingboardEntities = new List<PingboardEntity>();
            ApiCredentials apiCredentials = this.GetApiCredentials("PINGBOARD");
            string pingboardApiId = apiCredentials.APIID;
            string pingboardApiSecret = apiCredentials.APIKey;

            using (SqlConnection sqlConn = new SqlConnection("Server=SSIS17.haas.berkeley.edu;Database=HR;Integrated Security=True"))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = sqlConn.CreateCommand())
                {

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpClient httpClient = new HttpClient();
                    string pingboardTokenApiUrl = "https://app.pingboard.com/oauth/token?grant_type=client_credentials";
                    var pingboardTokenPostParams = new { client_id = pingboardApiId, client_secret = pingboardApiSecret };
                    var pingboardTokenResult = WebRequest. (Invoke - WebRequest - Method POST - Uri pingboardTokenApiUrl - Body pingboardTokenPostParams - ContentType "application/x-www-form-urlencoded" - UseBasicParsing).Content | ConvertFrom - Json
                    string pingboardToken = pingboardTokenResult.access_token;
                    var headers = new { Authorization = "Bearer pingboardToken" };

                    string pingboardUsersApiUrl = "https://app.pingboard.com/api/v2/users?page_size=100&page=1";
                    while (true)
                    {
                        var response = await httpClient.GetAsync(pingboardUsersApiUrl);
                        IEnumerable<PingboardUser>? pingboardUsers = await response.Content.ReadFromJsonAsync<IEnumerable<PingboardUser>>();
                        IQueryable<IEntity> destinationPingboardUsers = HRDataSourceManager.HRDbContext.PingboardUsers.Cast<IEntity>();
                        IList<PingboardUser> addPingboardUsers = (IList < PingboardUser > )this.GetDbSetInserts(pingboardUsers, destinationPingboardUsers);
                        HRDataSourceManager.HRDbContext.PingboardUsers.AddRange(addPingboardUsers);
                        IList<PingboardUser> updatePingboardUsers = (IList<PingboardUser>)this.GetDbSetUpdates(pingboardUsers, destinationPingboardUsers);
                        HRDataSourceManager.HRDbContext.PingboardUsers.AddRange(updatePingboardUsers);

                        foreach (PingboardUser user in pingboardUsers)
                        {
                            PingboardUser? existingPingboardUser = HRDataSourceManager.HRDbContext.PingboardUsers.Find(user.UID);
                            var userRow = dtPingboardUsers.NewRow();
                            DateTime callDateTime = DateTime.Now;
                            userRow.API_Call_Date = callDateTime.ToString("MM/dd/yyyy HH:mm:ss")
                            userRow.id = user.id
                            userRow.reports_to_id = user.reports_to_id
                            userRow.created_at = user.created_at
                            userRow.updated_at = user.updated_at
                            userRow.first_name = user.first_name
                            userRow.last_name = user.last_name
                            userRow.nickname = user.nickname
                            userRow.email = user.email
                            userRow.phone = user.phone
                            userRow.office_phone = user.office_phone
                            userRow.job_title = user.job_title
                            userRow.start_date = user.start_date
                            userRow.time_zone = user.time_zone
                            userRow.email_message_channel = user.email_message_channel
                            userRow.phone_message_channel = user.phone_message_channel
                            userRow.roles = [String]::Join('|', user.roles)
                            userRow.social_avatar_id = user.social_avatar_id
                            userRow.avatar_url = user.avatar_urls.large
                            userRow.locale = user.locale
                            userRow.Emp_Type_f1341030 = user.custom_fields.f1341030
                            userRow.UID_f1341029 = user.custom_fields.f1341029

                            userRow.Position_No_f3065450 = user.custom_fields.f3065450

                            userRow.Entity_ID_f3083343 = user.custom_fields.f3083343
                            //dtPingboardUsers.Rows.Add(userRow)
                            HRDataSourceManager.HRDbContext.PingboardUsers.Add(user);
                        }

                        if (String.IsNullOrWhiteSpace(result.meta.users.next_href))
                        {
                            break;
                        }
                        else
                        {
                            pingboardUsersApiUrl = "https://app.pingboard.com" + result.meta.users.next_href;
                        }

                        
                    }

                    HRDataSourceManager.HRDbContext.SaveChanges();

                    string pingboardGroupsApiUrl = "https://app.pingboard.com/api/v2/groups?page_size=100&page=1";
                    while (true)
                    {
                        resultGroups = (Invoke - WebRequest - Uri pingboardGroupsApiUrl - Headers headers - UseBasicParsing).Content | ConvertFrom - Json
                        write - host resultGroups.meta.users.next_href

                        foreach (group in resultGroups.groups)
                        {
                            DataRow groupRow = dtPingboardGroups.NewRow();
                            DateTime callDateTime = DateTime.Now;
                            groupRow.API_Call_Date = callDateTime.ToString("MM/dd/yyyy HH:mm:ss");
                            groupRow.id = group.id;
                            groupRow.name = group.name;
                            groupRow.description = group.description;
                            groupRow.created_at = group.created_at;
                            groupRow.updated_at = group.updated_at;
                            groupRow.memberships_count = group.memberships_count;
                            groupRow.logo_urls = group.logo_urls;
                            groupRow.type = group.type;
                            groupRow.visible_to_owner = group.visible_to_owner;
                            groupRow.visible_to_group = group.visible_to_group;
                            groupRow.visible_to_other = group.visible_to_other;
                            groupRow.editable_by_owner = group.editable_by_owner;
                            dtPingboardGroups.Rows.Add(groupRow);

                                foreach (userId in group.links.users)
                                        {
                                DataRow userGroupRow = dtPingboardUsersGroups.NewRow();
                                DateTime callDateTime = DateTime.Now;
                                userGroupRow.API_Call_Date = callDateTime.ToString("MM/dd/yyyy HH:mm:ss");
                                userGroupRow.user_id = userId;
                                userGroupRow.group_id = group.id;
                                dtPingboardUsersGroups.Rows.Add(userGroupRow);
                                userGroupRow.is_leader = 0;
                                }

                                foreach (var leader in group.leaders)
                                {
                                    if (leader.type - eq "User") {
                                        DataRow userGroupRow = dtPingboardUsersGroups.NewRow();
                                        DateTime callDateTime = DateTime.Now;
                                    userGroupRow.API_Call_Date = callDateTime.ToString("MM/dd/yyyy HH:mm:ss");
                                    userGroupRow.user_id = leader.id;
                                    userGroupRow.group_id = group.id;
                                    userGroupRow.is_leader = 1;
                                    dtPingboardUsersGroups.Rows.Add(userGroupRow);
                                    }
                                }
                            }

                            write - host resultGroups.meta.groups.next_href
                        if (String.IsNullOrWhiteSpace(resultGroups.meta.groups.next_href))
                        {
                                break;
                        }
                        else
                        {
                            //pingboardGroupsApiUrl = "https://app.pingboard.com" + resultGroups.meta.groups.next_href;
                        }
                    }
                }
            }

            return pingboardEntities;
        }
        */

        /*
        /// <summary>
        /// Helper method that returns a serialized JSON object based on a response
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PingboardUser>> GetSourcePingboardUsers(int pageIndex, int pageSize)
        {
            var response = await PingboardDataSource.HttpClient.GetAsync("https://app.pingboard.com/api/v2/groups?page_size=" + pageSize + "&page="  + pageIndex);
            IEnumerable <PingboardUser> result = await response.Content.ReadFromJsonAsync<IEnumerable<PingboardUser>>();
            return result;
        }
        */
    }
}
