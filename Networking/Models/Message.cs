/******************************************************************************
 * Filename    = Models/Message.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the structure of a message used in the networking system.
 *****************************************************************************/

using System;

namespace Networking.Models
{
    /// <summary>
    /// Represents a message in the networking system.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the data content of the message.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the destination module name associated with the message.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the destination ID of the message.
        /// </summary>
        public string DestID { get; set; }

        /// <summary>
        /// Gets or sets the sender ID of the message.
        /// </summary>
        public string SenderID { get; set; }

        /// <summary>
        /// Default constructor for the Message class.
        /// Initializes properties with empty strings.
        /// </summary>
        public Message()
        {
            Data = "";
            DestID = "";
            SenderID = "";
            ModuleName = "";
        }

        /// <summary>
        /// Parameterized constructor for the Message class.
        /// Initializes properties with the provided values.
        /// </summary>
        /// <param name="Data">The data content of the message.</param>
        /// <param name="ModuleName">The destination module name associated with the message.</param>
        /// <param name="destID">The destination ID of the message.</param>
        /// <param name="senderID">The sender ID of the message.</param>
        public Message( string Data , string ModuleName , string destID , string senderID )
        {
            this.ModuleName = ModuleName;
            this.Data = Data;
            DestID = destID;
            SenderID = senderID;
        }
    }
}
