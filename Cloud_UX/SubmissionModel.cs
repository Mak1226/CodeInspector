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
        private DownloadApi fileDownloadApi; //creating an instance of the FiledowloadApi.

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
            byte[] submissionBytes = await fileDownloadApi.GetSubmissionByUserNameAndSessionIdAsync(userName, sessionId);

            // If the submissionBytes is null, return an empty list
            if (submissionBytes == null)
            {
                return new List<SubmissionEntity>();
            }

            // Convert the bytes to a SubmissionEntity object
            SubmissionEntity submissionEntity = new SubmissionEntity(sessionId, userName);

            // You need to define a method to convert the byte array to your SubmissionEntity. Assuming a method called ConvertBytesToSubmissionEntity.
            // submissionEntity = ConvertBytesToSubmissionEntity(submissionBytes);

            // Assuming SubmissionsList is a property of your class
            SubmissionsList = new List<SubmissionEntity> { submissionEntity };

            return SubmissionsList;
        }

        /// <summary>
        /// For getting the path of user with respect to their local system.. 
        /// </summary>
        /// <returns>Return a path to download folder</returns>
        public static string GetDownloadFolderPath() //Getting the path to folder where the downloads folder contains. 
        {
            return System.Convert.ToString(
                Microsoft.Win32.Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty
                )
            );
        }

        /// <summary>
        /// Writes the file to the download folder. 
        /// </summary>
        /// <param name="num">Index in the submission list.</param>
        public void DownloadPdf(int num) //function for converting into pdf and write file at given download path. 
        {
            // byte[] pdf = SubmissionsList[num].Pdf;
            byte[] pdf =null ;
            string path = GetDownloadFolderPath() + "\\" + SubmissionsList[num].UserName + "_" + SubmissionsList[num].SessionId + ".txt";
            File.WriteAllBytes(path, pdf);
        }

      



    }
}
