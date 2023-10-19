/******************************************************************************
* Author      = Susan
*
* Product     = Analyzer
* 
* Project     = Content
*
* Description = Class to encode DLL files
*****************************************************************************/
using System.Text;
using System.Xml;

namespace Content
{
    /// <summary>
    /// Class to encode and decode DLL files to send over network
    /// </summary>
    internal class DLLEncoder : IFileEncoder
    {
        private readonly Dictionary<string, string> _data; // The content of the file as strings

        /// <summary>
        /// Initialize a DLLEncoder
        /// </summary>
        public DLLEncoder()
        {
            _data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Encodes the DLL data into an XML format.
        /// </summary>
        /// <param name="filePaths">Path to each file to be encoded together. Need to be DLLs</param>
        /// <returns>An XML representation of the DLL data as a string.</returns>
        public string GetEncoded(List<string> filePaths)
        {
            if (filePaths == null || filePaths.Count == 0)
            {
                throw new Exception("No file found");
            }

            // initialize the xml Document
            XmlDocument xmlDocument = new ();
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(root);

            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    // Read the content of the file as a string
                    string fileContent = File.ReadAllText(filePath, Encoding.UTF8);

                    // Create a child element for the file
                    XmlElement fileElement = xmlDocument.CreateElement("File");

                    // Add an attribute for the file name/path
                    XmlAttribute nameAttribute = xmlDocument.CreateAttribute("Name");
                    nameAttribute.Value = filePath;
                    fileElement.Attributes.Append(nameAttribute);

                    // Set the content of the file element
                    fileElement.InnerText = fileContent;

                    // Add the file element to the root
                    root.AppendChild(fileElement);
                }

                return xmlDocument.OuterXml;
            }
            return "";
        }

        /// <summary>
        /// Decodes DLL data from an XML string and processes the extracted information.
        /// </summary>
        /// <param name="xmlData">The XML data representing the DLL content as a string.</param>
        public void DecodeFrom(string xmlData)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);

            XmlNode? root = xmlDocument.SelectSingleNode("Root");

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
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saves the files from the dictionary data into the path given as input
        /// </summary>
        /// <param name="path"></param>
        public void SaveFiles(string path)
        {
            throw new NotImplementedException();
        }
    }
}
