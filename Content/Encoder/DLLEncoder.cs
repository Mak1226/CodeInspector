/******************************************************************************
 * Filename    = DLLEncoder.cs
 * 
 * Author      = Susan
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Class to encode DLL files
 *****************************************************************************/
using System.Diagnostics;
using System.Xml;

namespace Content.Encoder
{
    /// <summary>
    /// Class to encode and decode DLL files to send over network
    /// </summary>
    internal class DLLEncoder : IFileEncoder
    {
        private readonly Dictionary<string, string> _data; // The content of the file as strings

        public string sessionID { get; private set; }

        /// <summary>
        /// Initialize a DLLEncoder for a user session
        /// </summary>
        public DLLEncoder()
        {
            Trace.WriteLine( "[Content][DLLEncoder.cs] : DLLEncoder Initialized" );
            _data = new Dictionary<string, string>();
            sessionID = string.Empty;
        }

        /// <summary>
        /// Encodes the DLL data into an XML format.
        /// </summary>
        /// <param name="filePaths">Path to each file to be encoded together. Need to be DLLs</param>
        /// <returns>An XML representation of the DLL data as a string.</returns>
        public string GetEncoded(List<string> filePaths, string rootPath, string sessionID)
        {
            Trace.WriteLine( "[Content][DLLEncoder.cs] : GetEncoded" );
            this.sessionID = sessionID;

            if (filePaths == null || filePaths.Count == 0)
            {
                throw new Exception("No file found");
            }

            // initialize the xml Document
            XmlDocument xmlDocument = new ();
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(root);

            // Encode the user session ID
            XmlAttribute sessionAttribute = xmlDocument.CreateAttribute("SessionID");
            sessionAttribute.Value = sessionID;
            root.Attributes.Append(sessionAttribute);

            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    // Read the content of the file as a string
                    //string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string fileContent = Convert.ToBase64String(fileBytes);

                    // Create a child element for the file
                    XmlElement fileElement = xmlDocument.CreateElement("File");

                    // Add an attribute for the file name/path
                    XmlAttribute nameAttribute = xmlDocument.CreateAttribute("Name");
                    nameAttribute.Value = Path.GetRelativePath(rootPath, filePath);
                    fileElement.Attributes.Append(nameAttribute);

                    // Set the content of the file element
                    fileElement.InnerText = fileContent;

                    // Add the file element to the root
                    root.AppendChild(fileElement);
                }

            }
            return xmlDocument.OuterXml;
        }

        /// <summary>
        /// Decodes DLL data from an XML string and processes the extracted information.
        /// </summary>
        /// <param name="xmlData">The XML data representing the DLL content as a string.</param>
        public void DecodeFrom(string xmlData)
        {
            Trace.WriteLine( "[Content][DLLEncoder.cs] : DecodeFrom" );
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);

            XmlNode? root = xmlDocument.SelectSingleNode("Root");

            _data.Clear();

            // Decode session ID
            XmlAttribute sessionAttribute = root.Attributes["SessionID"];
            if (sessionAttribute != null)
            {
                sessionID = sessionAttribute.Value;
            }

            if(root != null)
            {
                foreach (XmlNode fileElement in root.ChildNodes)
                {
                    if (fileElement is XmlElement)
                    {
                        XmlAttribute? nameAttribute = fileElement.Attributes["Name"];
                        if (nameAttribute != null)
                        {
                            string filePath = nameAttribute.Value;
                            string fileContent = fileElement.InnerText;

                            // Add the file content to the dictionary with the file path as the key
                            _data[filePath] = fileContent;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Function returns the private variable data. This is only needed for debugging purposes till the SaveFiles function is implemented.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string ,string> GetData()
        {
            return _data;
        }

        /// <summary>
        /// Saves the files from the dictionary data into the path given as input
        /// </summary>
        /// <param name="path"></param>
        public void SaveFiles(string path)
        {
            Trace.WriteLine( "[Content][DLLEncoder.cs] : DLLEncoder SaveFiles" );
            if (_data == null)
            {
                throw new ArgumentNullException( nameof( _data ) , "Data dictionary is not initialized." );
            }

            if (string.IsNullOrEmpty( path ))
            {
                throw new ArgumentNullException( nameof( path ) , "Path is not specified." );
            }

            if (!Directory.Exists( path ))
            {
                Directory.CreateDirectory( path );
            }
            DirectoryInfo directoryInfo = new( path );

            // Delete all files in the directory
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            // Delete all subdirectories and their contents
            foreach (DirectoryInfo subdirectory in directoryInfo.GetDirectories())
            {
                subdirectory.Delete( true );
            }
            foreach (KeyValuePair<string, string> kvp in _data)
            {
                string filePath = Path.Combine( path , kvp.Key );
                FileInfo fileInfo = (new FileInfo(filePath));
                fileInfo.Directory.Create(); // Ensure that directory exists
                //File.WriteAllText( filePath , kvp.Value, new UTF8Encoding(false) );

                byte[] fileBytes = Convert.FromBase64String(kvp.Value);
                File.WriteAllBytes( filePath, fileBytes );
            }

        }

    }
}
