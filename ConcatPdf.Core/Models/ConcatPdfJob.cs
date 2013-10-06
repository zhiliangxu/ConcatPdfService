using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ConcatPdf.Core.Models
{
    public class ConcatPdfJob
    {
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }

        [JsonProperty("inputUris")]
        public List<string> InputUris
        {
            get;
            set;
        }

        [JsonProperty("outputUri")]
        public string OutputUri
        {
            get;
            set;
        }

        [JsonProperty("submittedDate")]
        public DateTime SubmittedDate
        {
            get;
            set;
        }

        [JsonProperty("outputFile")]
        public string OutputFile
        { 
            get;
            set;
        }

        [JsonProperty("status")]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty("processedDate")]
        public DateTime ProcessedDate
        {
            get;
            set;
        }

        [JsonProperty("processingAttempts")]
        public int ProcessingAttempts
        {
            get;
            set;
        }

        [JsonIgnore]
        public string ETag
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTimeOffset Timestamp
        {
            get;
            set;
        }
    }
}
