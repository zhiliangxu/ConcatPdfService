using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Implementations
{
    internal class WindowsAzureTableClient
    {
        public const string ConcatPdfJobTableName = "ConcatPdfJob";

        private CloudTableClient tableClient;

        public WindowsAzureTableClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task InsertAsync(string tableName, ITableEntity entity)
        {
            TableOperation operation = TableOperation.Insert(entity);
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.ExecuteAsync(operation);
        }

        public async Task<ITableEntity> MergeAsync(string tableName, ITableEntity entity)
        {
            TableOperation operation = TableOperation.Merge(entity);
            CloudTable table = tableClient.GetTableReference(tableName);
            TableResult result = await table.ExecuteAsync(operation);
            entity.ETag = result.Etag;
            return entity;
        }

        public async Task InsertOrReplaceAsync(string tableName, ITableEntity entity)
        {
            TableOperation operation = TableOperation.InsertOrReplace(entity);
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.ExecuteAsync(operation);
        }

        public async Task InsertOrMergeAsync(string tableName, ITableEntity entity)
        {
            TableOperation operation = TableOperation.InsertOrMerge(entity);
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.ExecuteAsync(operation);
        }

        public async Task DeleteAsync(string tableName, ITableEntity entity)
        {
            TableOperation operation = TableOperation.Delete(entity);
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.ExecuteAsync(operation);
        }

        public IEnumerable<T> SelectAsync<T>(string tableName, TableQuery<T> query)
            where T : ITableEntity, new()
        {
            CloudTable table = tableClient.GetTableReference(tableName);
            return table.ExecuteQuery<T>(query);
        }

        public async Task<T> RetrieveAsync<T>(string tableName, string partitionKey, string rowKey)
            where T : class, ITableEntity
        {
            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            return (await table.ExecuteAsync(operation)).Result as T;
        }
    }
}
