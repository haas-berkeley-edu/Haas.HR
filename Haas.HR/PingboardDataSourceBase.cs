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
    }
}
