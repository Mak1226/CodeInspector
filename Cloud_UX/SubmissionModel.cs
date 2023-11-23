/******************************************************************************
 * Filename    = SubmissionModel.cs
 *
 * Author      = Sidharth Chadha
 * 
 * Project     = Cloud_Ux
 *
 * Description = Created Model for the downloading functionality. 
 *****************************************************************************/

using ServerlessFunc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Cloud_UX
{
    public class SubmissionsModel
    {
        //getting path from the files
        string[] paths;
        private string analysisUrl = "http://localhost:7074/api/analysis";
        private string submissionUrl = "http://localhost:7074/api/submission";
        private string sessionUrl = "http://localhost:7074/api/session";
        public DownloadApi fileDownloadApi; //creating an instance of the FiledowloadApi.
        
        public SubmissionsModel() //constructor for the submissionmodel class. 
        {
            fileDownloadApi = new DownloadApi(sessionUrl, submissionUrl, analysisUrl);
        }

        public IReadOnlyList<SubmissionEntity>? SubmissionsList; //creating the submission list to store the details of type submission model. 

        /// <summary>
        /// uses the async function to reterieve the file from the cloud. 
        /// </summary>
        /// <param name="sessionId">Unique id for a session</param>
        /// <returns>Returns the submission entity for given session id</returns>
        public async Task<IReadOnlyList<SubmissionEntity>> GetSubmissions(string sessionId, string userName)
        {
            // Call the API to get the submission bytes
            byte [] submissionBytes = await fileDownloadApi.GetSubmissionByUserNameAndSessionIdAsync(userName, sessionId);

            // If the submissionBytes is null, return an empty list
            if (submissionBytes == null)
            {
                return new List<SubmissionEntity>();
            }

            // Convert the bytes to a SubmissionEntity object
            SubmissionEntity submissionEntity = new SubmissionEntity(sessionId, userName);
            
         
            SubmissionsList = new List<SubmissionEntity> { submissionEntity };

            return SubmissionsList;
        }

        /// <summary>
        /// For getting the path of user with respect to their local system.. 
        public static string GetDownloadFolderPath() //Getting the path to folder where the downloads folder contains. 
        {
            // to do -> change the dowload path
            return @"C:\Users\sidha\Downloads\download_cloud";
        }

        /// <summary>
        /// Writes the file to the download folder. 
        /// </summary>
        /// <param name="num">Index in the submission list.</param>
        public async void DownloadPdf(int num) //function for converting into txt and write file at given download path. 
        {
            byte[] file_Data = await fileDownloadApi.GetSubmissionByUserNameAndSessionIdAsync(SubmissionsList[num].UserName, SubmissionsList[num].SessionId);
            string path = GetDownloadFolderPath() + "\\" + SubmissionsList[num].UserName + "_" + SubmissionsList[num].SessionId + ".txt";
            File.WriteAllBytes(path, file_Data);
            Trace.WriteLine("file saved to local path");
        }

      



    }
}
