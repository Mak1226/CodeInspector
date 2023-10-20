/******************************************************************************
* Author      = Susan
*
* Product     = Analyzer
* 
* Project     = Content
*
* Description = Interface for encoding and decoding files to send over network
*****************************************************************************/

namespace Content
{
    /// <summary>
    /// Represents an interface for encoding and decoding data.
    /// </summary>
    internal interface IFileEncoder
    {
        /// <summary>
        /// Encodes all files given as input into a single XML encoding
        /// </summary>
        /// <param name="filePaths">Path to each file to be encoded together</param>
        /// <returns>A string containing the encoded XML data.</returns>
        string GetEncoded(List<string> filePaths);

        /// <summary>
        /// Decodes data from an XML string and sets the internal state of an object with the decoded data.
        /// </summary>
        /// <param name="packet">The XML string containing the data to be decoded.</param>
        void DecodeFrom(string packet);

        /// <summary>
        /// This function can be used to access the private variable data.
        /// </summary>
        /// <returns>The class variable data that contains file data</returns>
        Dictionary<string , string> GetData();

        /// <summary>
        /// Save all data in internal state into the required filepaths.
        /// Note: filepaths are encoded in the XML
        /// </summary>
        /// <param name="path">Directory root to where each file is to be saved</param>
        void SaveFiles(string path);
    }
}
