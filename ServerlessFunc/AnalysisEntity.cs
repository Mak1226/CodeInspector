using Azure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Azure.Data.Tables;
using ITableEntity = Azure.Data.Tables.ITableEntity;
using System.ComponentModel.DataAnnotations;

namespace ServerlessFunc
{
    public class AnalysisEntity : ITableEntity
    {
        public const string PartitionKeyName = "AnalysisEntityPartitionKey";
        /// <summary>
        /// To store the sessionId of the submission
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("SessionId")]
        public string SessionId { get; set; }
        /// <summary>
        /// To store the username of the person submitted.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// To store the analysis file.
        /// </summary> 
        [JsonInclude]
        [JsonPropertyName("AnalysisFile")]
        public byte[] AnalysisFile { get; set; }
        /// <summary>
        /// To store the unique id for the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Id")]
        public string Id { get; set; }
        /// <summary>
        /// To store the partition key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("PartitionKey")]
        public string PartitionKey { get; set; }
        /// <summary>
        /// To store the row key.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("RowKey")]
        public string RowKey { get; set; }

        /// <summary>
        /// To store the timestamp of the entry.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        public AnalysisEntity(AnalysisData analysisData)
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
        public AnalysisEntity() : this(null) { }

        [JsonIgnore]
        public ETag ETag { get; set; }

    }

    public class AnalysisData
    {
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public byte[] AnalysisFile { get; set; }

    }
}
