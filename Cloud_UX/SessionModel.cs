/******************************************************************************
 * Filename    = SessionModel.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = Cloud_UX
 *
 * Description = Model logic for the sessions. 
 *****************************************************************************/

using ServerlessFunc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Cloud_UX
{
    public class SessionsModel
    {
        string[] paths; //string array to store the read content of the url file. 
        private string analysisUrl = "http://localhost:7074/api/analysis";
        private string submissionUrl = "http://localhost:7074/api/submission";
        private string sessionUrl = "http://localhost:7074/api/session";
        private DownloadApi fileDownloadApi;
        public SessionsModel()
        {
            Trace.WriteLine("[cloud] New DownloadApi instance created");
            fileDownloadApi = new DownloadApi(sessionUrl, submissionUrl, analysisUrl);
        }
        /// <summary>
        /// Takes the username and retreive the information from the session table. 
        /// </summary>
        /// <param name="userName">Username of the user we consider</param>
        /// <returns>will return the session entity for given username.</returns>
        public async Task<IReadOnlyList<SessionEntity>> GetSessionsDetails(string userName)
        {
            IReadOnlyList<SessionEntity>? getEntity = await fileDownloadApi.GetSessionsByHostNameAsync(userName);
            Trace.WriteLine("[cloud] Retrieved all session details for " + userName);
            return getEntity;
        }
        
    }
}
