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
        /// Encodes data and returns it as a string.
        /// </summary>
        /// <returns>A byte array containing the encoded data.</returns>
        string GetEncoded();

        /// <summary>
        /// Decodes data from a string and sets the internal state of an object with the decoded data.
        /// </summary>
        /// <param name="packet">The byte array containing the data to be decoded.</param>
        void DecodeFrom(string xmlData);
    }
}
