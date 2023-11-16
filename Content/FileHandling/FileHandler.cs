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
using Networking.Utils;
using Networking.Communicator;
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
        private ICommunicator _fileSender;
        private readonly Dictionary<string, string> _files;
        private readonly IFileEncoder _fileEncoder;
        /// <summary>
        /// saves files in //data/
        /// </summary>
        public FileHandler(ICommunicator fileSender)
        {
            _files = new Dictionary<string, string>();
            _fileEncoder = new DLLEncoder();
            _filesList = new List<string>();
            _fileSender = fileSender;
        }
        /// <summary>
        /// Retrieves a list of file paths representing the files stored or managed by the file handler.
        /// </summary>
        /// <returns>A list of file paths as strings.</returns>
        public List<string> GetFiles()
        {
            return _filesList; 
        }

        /// <summary>
        /// Saves file in data location and calls analyzer query
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sessionID"></param>
        public void Upload(string filepath, string sessionID)
        {
            // extract dll , and pass it to xml encoder use network functions to send
            // extracting paths of all dll files from the given directory
            string[] dllFiles = Directory.GetFiles(filepath, "*.dll", SearchOption.AllDirectories);
            string encoding = _fileEncoder.GetEncoded(dllFiles.ToList(), filepath, sessionID);

            // Encode RecieveEventType
            Dictionary<string, string> sendData = new()
            {
                { "EventType", "File" },
                { "Data", encoding }
            };
            encoding = Serializer.Serialize(sendData);

            _filesList = dllFiles.ToList();
            Trace.Write(encoding);
            _fileSender.Send(encoding, "Content-Files", "server");
        }

        /// <summary>
        /// To be called with 
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public void HandleRecieve(string encoding)
        {
            Dictionary<string, string> recvData;
            try
            {
                recvData = Serializer.Deserialize<Dictionary<string, string>>(encoding);
            }
            catch (JsonException) 
            {
                return;
            }
            if (recvData["EventType"] != "File")
            {
                // Packet not meant for this module
                return;
            }

            encoding = recvData["Data"];
            _fileEncoder.DecodeFrom(encoding);
            string sessionID = _fileEncoder.sessionID;
            string sessionPath = sessionID;

            _fileEncoder.SaveFiles(sessionPath);
            Dictionary<string, string> decodedFiles = _fileEncoder.GetData();
            _filesList = new List<String>();
           foreach (var file in decodedFiles)
            {
                _filesList.Add(Path.Combine(sessionPath, file.Key));
            }
        }
    }
}
