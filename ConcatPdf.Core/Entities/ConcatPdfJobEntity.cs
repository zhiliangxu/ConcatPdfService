using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace ConcatPdf.Core.Entities
{
    public class ConcatPdfJobEntity : TableEntity
    {
        public const string DefaultPartitionKey = "job";

        public string Id
        {
            get;
            set;
        }

        public string InputUris
        {
            get;
            set;
        }

        public string OutputFile
        {
            get;
            set;
        }

        public string OutputUri
        {
            get;
            set;
        }

        public DateTime SubmittedDate
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public DateTime ProcessedDate
        {
            get;
            set;
        }

        public int ProcessingAttempts
        {
            get;
            set;
        }

        public void FillPartitionKeyAndRowKey()
        {
            this.PartitionKey = DefaultPartitionKey;
            this.RowKey = this.Id;
        }
    }
}
