/******************************************************************************
* Filename    = SubmissionEntity.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Model for the entity which stores the structure of submission file.
*****************************************************************************/

using System;
using System.Text.Json.Serialization;
using Azure;
using ITableEntity = Azure.Data.Tables.ITableEntity;

namespace ServerlessFunc
{
    /// <summary>
    /// Represents an entity for storing submission information.
    /// </summary>
    public class SubmissionEntity : ITableEntity
    {
        /// <summary>
        /// The partition key name for SubmissionEntity.
        /// </summary>
        public const string PartitionKeyName = "SubmissionEntityPartitionKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmissionEntity"/> class.
        /// </summary>
        /// <param name="sessionId">The session ID of the submission.</param>
        /// <param name="username">The username of the user who submitted the file.</param>
        public SubmissionEntity( string sessionId , string username )
        {
            PartitionKey = PartitionKeyName;
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;
            SessionId = sessionId;
            UserName = username;
            BlobName = sessionId + '/' + username;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmissionEntity"/> class.
        /// </summary>
        public SubmissionEntity() : this( null , null ) { }

        /// <summary>
        /// The session ID of the submission.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "SessionId" )]
        public string SessionId { get; set; }

        /// <summary>
        /// The blob name associated with the submission.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "BlobName" )]
        public string BlobName { get; set; }

        /// <summary>
        /// The unique identifier for the SubmissionEntity.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Id" )]
        public string Id { get; set; }

        /// <summary>
        /// The username of the user who submitted the file.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "UserName" )]
        public string UserName { get; set; }

        /// <summary>
        /// The partition key for storage.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "PartitionKey" )]
        public string PartitionKey { get; set; }

        /// <summary>
        /// The row key for storage.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "RowKey" )]
        public string RowKey { get; set; }

        /// <summary>
        /// The timestamp of the submission.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Timestamp" )]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The ETag for concurrency control.
        /// </summary>
        [JsonIgnore]
        public ETag ETag { get; set; }
    }

    /// <summary>
    /// Represents the data associated with a submission.
    /// </summary>
    public class SubmissionData
    {
        /// <summary>
        /// The session ID for the submission.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The username of the user who submitted the file.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The zipped DLL files for the submission.
        /// </summary>
        public byte[] ZippedDllFiles { get; set; }
    }
}
