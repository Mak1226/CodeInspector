using Azure;
using System;
using System.Text.Json.Serialization;
using ITableEntity = Azure.Data.Tables.ITableEntity;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace ServerlessFunc
{
    public class SessionEntity : ITableEntity
    {
        public const string PartitionKeyName = "SessionEntityPartitionKey";

        public SessionEntity(SessionData sessionData = null)
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

        public SessionEntity() : this(null) { }

        /// <summary>
        /// To store session Id.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("SessionId")]
        public string SessionId { get; set; }


        /// <summary>
        /// To store Unique id for the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Id")]
        public string Id { get; set; }


        /// <summary>
        /// To store the host user name.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("HostUserName")]
        public string HostUserName { get; set; }

        [JsonInclude]
        [JsonPropertyName("PartitionKey")]
        public string PartitionKey { get; set; }

        [JsonInclude]
        [JsonPropertyName("RowKey")]
        public string RowKey { get; set; }

        /// <summary>
        /// To store start Timestamp of the session.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// To store the tests done in session
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Tests")]
        public byte[] Tests { get; set; }

        /// <summary>
        /// To store the list of studnets joined in session
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Students")]
        public byte[] Students { get; set; }

        [JsonInclude]
        [JsonPropertyName("TestNameToID")]
        public byte[] TestNameToID { get; set; }

        [JsonIgnore]
        public ETag ETag { get; set; }
    }

    public class SessionData
    {

        public string HostUserName { get; set; }

        public string SessionId { get; set; }

        public byte[] Tests { get; set; }
        public byte[] Students { get; set; }

        public byte[] TestNameToID { get; set; }
    }
}

