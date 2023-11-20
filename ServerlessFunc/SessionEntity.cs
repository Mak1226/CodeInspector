using System;
using System.Text.Json.Serialization;
using Azure;
using ITableEntity = Azure.Data.Tables.ITableEntity;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace ServerlessFunc
{
    /// <summary>
    /// Represents an entity for storing session information.
    /// </summary>
    public class SessionEntity : ITableEntity
    {
        /// <summary>
        /// The partition key name for SessionEntity.
        /// </summary>
        public const string PartitionKeyName = "SessionEntityPartitionKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionEntity"/> class.
        /// </summary>
        /// <param name="sessionData">The data associated with the session.</param>
        public SessionEntity( SessionData sessionData = null )
        {
            PartitionKey = PartitionKeyName;
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;

            if (sessionData != null)
            {
                SessionId = sessionData.SessionId;
                HostUserName = sessionData.HostUserName;
                Tests = sessionData.Tests;
                Students = sessionData.Students;
                TestNameToID = sessionData.TestNameToID;
            }

            Timestamp = DateTime.Now;
            ETag = new ETag();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionEntity"/> class.
        /// </summary>
        public SessionEntity() : this( null ) { }

        /// <summary>
        /// To store the session Id.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "SessionId" )]
        public string SessionId { get; set; }

        /// <summary>
        /// To store the unique identifier for the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Id" )]
        public string Id { get; set; }

        /// <summary>
        /// To store the host username.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "HostUserName" )]
        public string HostUserName { get; set; }

        /// <summary>
        /// To store the partition key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "PartitionKey" )]
        public string PartitionKey { get; set; }

        /// <summary>
        /// To store the row key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "RowKey" )]
        public string RowKey { get; set; }

        /// <summary>
        /// To store the start timestamp of the session.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Timestamp" )]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// To store the tests conducted in the session.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Tests" )]
        public byte[] Tests { get; set; }

        /// <summary>
        /// To store the list of students joined in the session.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Students" )]
        public byte[] Students { get; set; }

        /// <summary>
        /// To store the mapping of test names to IDs.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "TestNameToID" )]
        public byte[] TestNameToID { get; set; }

        /// <summary>
        /// The ETag for concurrency control.
        /// </summary>
        [JsonIgnore]
        public ETag ETag { get; set; }
    }

    /// <summary>
    /// Represents the data associated with a session.
    /// </summary>
    public class SessionData
    {
        /// <summary>
        /// The host username for the session.
        /// </summary>
        public string HostUserName { get; set; }

        /// <summary>
        /// The session ID.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The byte array representing the tests conducted in the session.
        /// </summary>
        public byte[] Tests { get; set; }

        /// <summary>
        /// The byte array representing the list of students joined in the session.
        /// </summary>
        public byte[] Students { get; set; }

        /// <summary>
        /// The byte array representing the mapping of test names to IDs.
        /// </summary>
        public byte[] TestNameToID { get; set; }
    }
}
