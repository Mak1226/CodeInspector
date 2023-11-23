/******************************************************************************
 * Filename    = FileHandler.cs
 * 
 * Author      = Lekshmi
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Class that implements IFileHandler
 *****************************************************************************/

using Content.Encoder;
using System.Diagnostics;
using Networking.Serialization;
using System.Text.Json;

namespace Content.FileHandling
{
    /// <summary>
    /// Currently implemented as simply copying a file to the 
    /// context of the running application
    /// </summary>
    public class FileHandler : IFileHandler
    {
        private List<string> _filesList;
        private readonly IFileEncoder _fileEncoder;

        /// <summary>
        /// Constructor for filehandler class. 
        /// </summary>
        public FileHandler()
        {
            Trace.WriteLine( "Content: FileHandler.cs: FileHandler: Started" );
            _fileEncoder = new DLLEncoder();
            _filesList = new List<string>();
        }

        /// <summary>
        /// Retrieves a list of file paths representing the files stored or managed by the file handler.
        /// </summary>
        /// <returns>A list of file paths as strings.</returns>
        public List<string> GetFiles()
        {
            Trace.WriteLine( "Content: FileHandler.cs: GetFiles" );
            return _filesList;

        }

        /// <summary>
        /// Processes file upload, saves the file in the data location, 
        /// and generates an encoded representation for analysis.
        /// </summary>
        /// <param name="filepath">The path of the file or directory to be uploaded.</param>
        /// <param name="sessionID">The session ID associated with the upload.</param>
        /// <returns>The encoded representation of the file data for further analysis.</returns>
        public string HandleUpload(string filepath, string sessionID)
        {
            List<string> dllFiles = new();
            // extract dll , and pass it to xml encoder use network functions to send
            // extracting paths of all dll files from the given directory
            string encoding;
            if (Directory.Exists(filepath))
            {
                try
                {
                    dllFiles = Directory.GetFiles(filepath, "*.dll", SearchOption.AllDirectories).ToList();
                    encoding = _fileEncoder.GetEncoded(dllFiles, filepath, sessionID);
                }
                catch
                {
                    encoding = "";
                }
            }
            // Check if the path is a file
            else if (File.Exists(filepath) && string.Equals(Path.GetExtension(filepath), ".dll", StringComparison.OrdinalIgnoreCase))
            {
                dllFiles = new List<string> { filepath };
                encoding = _fileEncoder.GetEncoded(dllFiles.ToList(), Path.GetDirectoryName(filepath), sessionID);
                // Do something specific for files
            }
            else
            {
                return "";
            }

            Trace.Write(encoding);
            // Encode RecieveEventType
            Dictionary<string, string> sendData = new()
            {
                { "EventType", "File" },
                { "Data", encoding }
            };
            encoding = Serializer.Serialize(sendData);

            _filesList = dllFiles;
            Trace.Write(encoding);
            return encoding;
        }

        /// <summary>
        /// Processes received encoded data, decodes files, 
        /// and saves them in the specified session path.
        /// </summary>
        /// <param name="encoding">The encoded data to be processed.</param>
        /// <returns>The session ID associated with the received data, 
        /// or null if the decoding fails or the event type is not "File".</returns>
        public string? HandleRecieve(string encoding)
        {
            Dictionary<string, string> recvData;
            _filesList = new List<string>();
            try
            {
                recvData = Serializer.Deserialize<Dictionary<string, string>>(encoding);
            }
            catch (JsonException) 
            {
                return null;
            }
            if (recvData["EventType"] != "File")
            {
                // Packet not meant for this module
                return null;
            }

            encoding = recvData["Data"];
            _fileEncoder.DecodeFrom(encoding);
            string sessionID = _fileEncoder.sessionID;
            string sessionPath = sessionID;

            _fileEncoder.SaveFiles(sessionPath);
            Dictionary<string, string> decodedFiles = _fileEncoder.GetData();
            _filesList = new List<string>();
            foreach (KeyValuePair<string , string> file in decodedFiles)
            {
                _filesList.Add(Path.Combine(sessionPath, file.Key));
            }

            return sessionID;
        }
    }
}
