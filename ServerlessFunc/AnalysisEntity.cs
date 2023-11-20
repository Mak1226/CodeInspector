using System;
using System.Text.Json.Serialization;
using Azure;
using ITableEntity = Azure.Data.Tables.ITableEntity;

namespace ServerlessFunc
{
    /// <summary>
    /// Represents an entity for storing analysis data in Azure Table Storage.
    /// </summary>
    public class AnalysisEntity : ITableEntity
    {
        /// <summary>
        /// The name of the partition key for AnalysisEntity.
        /// </summary>
        public const string PartitionKeyName = "AnalysisEntityPartitionKey";

        /// <summary>
        /// The sessionId of the submission.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "SessionId" )]
        public string SessionId { get; set; }

        /// <summary>
        /// The username of the person who submitted the analysis.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "UserName" )]
        public string UserName { get; set; }

        /// <summary>
        /// The analysis file as a byte array.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "AnalysisFile" )]
        public byte[] AnalysisFile { get; set; }

        /// <summary>
        /// The unique ID for the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Id" )]
        public string Id { get; set; }

        /// <summary>
        /// The partition key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "PartitionKey" )]
        public string PartitionKey { get; set; }

        /// <summary>
        /// The row key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "RowKey" )]
        public string RowKey { get; set; }

        /// <summary>
        /// The timestamp of the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName( "Timestamp" )]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the AnalysisEntity class with the specified AnalysisData.
        /// </summary>
        /// <param name="analysisData">The analysis data to create the new analysis entity with.</param>
        public AnalysisEntity( AnalysisData analysisData )
        {
            PartitionKey = PartitionKeyName;
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;

            if (analysisData != null)
            {
                SessionId = analysisData.SessionId;
                UserName = analysisData.UserName;
                AnalysisFile = analysisData.AnalysisFile;
            }

            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the AnalysisEntity class.
        /// </summary>
        public AnalysisEntity() : this( null ) { }

        /// <inheritdoc/>
        [JsonIgnore]
        public ETag ETag { get; set; }
    }

    /// <summary>
    /// Represents the data associated with an analysis.
    /// </summary>
    public class AnalysisData
    {
        /// <summary>
        /// The sessionId associated with the analysis.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The username associated with the analysis.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The analysis file as a byte array.
        /// </summary>
        public byte[] AnalysisFile { get; set; }
    }
}
