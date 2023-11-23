/******************************************************************************
 * Filename    = Models/Message.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the structure of a message used in the networking system.
 *****************************************************************************/

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
        public string DestId { get; set; }

        /// <summary>
        /// Gets or sets the sender ID of the message.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Default constructor for the Message class.
        /// Initializes properties with empty strings.
        /// </summary>
        public Message()
        {
            Data = "";
            DestId = "";
            SenderId = "";
            ModuleName = "";
        }

        /// <summary>
        /// Parameterized constructor for the Message class.
        /// Initializes properties with the provided values.
        /// </summary>
        /// <param name="Data">The data content of the message.</param>
        /// <param name="ModuleName">The destination module name associated with the message.</param>
        /// <param name="destId">The destination ID of the message.</param>
        /// <param name="senderId">The sender ID of the message.</param>
        public Message( string Data , string ModuleName , string destId , string senderId )
        {
            this.ModuleName = ModuleName;
            this.Data = Data;
            DestId = destId;
            SenderId = senderId;
        }
    }
}
