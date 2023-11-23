/******************************************************************************
 * Filename    = Models/Data.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the structure of Data object used by networking module.
 *****************************************************************************/

namespace Networking.Models
{
    /// <summary>
    /// Represents data object for networking module.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets or sets the event type associated with the data.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the payload associated with the data.
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Default constructor for the Data class.
        /// Initializes properties with empty strings.
        /// </summary>
        public Data()
        {
            EventType = "";
            Payload = "";
        }

        /// <summary>
        /// Parameterized constructor for the Data class with only event type.
        /// Initializes EventType with the provided value.
        /// </summary>
        /// <param name="EventType">The event type associated with the data.</param>
        public Data( string EventType )
        {
            this.EventType = EventType;
        }

        /// <summary>
        /// Parameterized constructor for the Data class with payload and event type.
        /// Initializes Payload and EventType with the provided values.
        /// </summary>
        /// <param name="payload">The payload associated with the data.</param>
        /// <param name="EventType">The event type associated with the data.</param>
        public Data( string payload , string EventType )
        {
            Payload = payload;
            this.EventType = EventType;
        }
    }
}
