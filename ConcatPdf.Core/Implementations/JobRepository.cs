using ConcatPdf.Core.Entities;
using ConcatPdf.Core.Interfaces;
using ConcatPdf.Core.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Implementations
{
    public class JobRepository : IJobRepository
    {
        WindowsAzureTableClient tableClient = new WindowsAzureTableClient();
        AzureCloudQueueClient queueClient = new AzureCloudQueueClient();

        public async Task CreateJobAsync(ConcatPdfJob cpj)
        {
            ConcatPdfJobEntity entity = cpj.ToEntity();
            entity.FillPartitionKeyAndRowKey();
            await tableClient.InsertAsync(WindowsAzureTableClient.ConcatPdfJobTableName, entity);
        }

        public async Task UpdateJobAsync(ConcatPdfJob cpj)
        {
            ConcatPdfJobEntity entity = cpj.ToEntity();
            entity.FillPartitionKeyAndRowKey();
            ITableEntity resultEntity = await tableClient.MergeAsync(WindowsAzureTableClient.ConcatPdfJobTableName, entity);
            cpj.ETag = resultEntity.ETag;
        }

        public async Task<ConcatPdfJob> GetJobAsync(string id)
        {
            ConcatPdfJobEntity entity = await tableClient.RetrieveAsync<ConcatPdfJobEntity>(WindowsAzureTableClient.ConcatPdfJobTableName,
                ConcatPdfJobEntity.DefaultPartitionKey, id);
            return entity.ToModel();
        }

        public async Task EnqueueJobAsync(string jobId)
        {
            await queueClient.EnqueueAsync(AzureCloudQueueClient.ConcatPdfJobQueueName, jobId);
        }

        public async Task<IEnumerable<Tuple<string, string, string>>> GetJobsFromQueueAsync()
        {
            return await queueClient.GetMessagesAsync(AzureCloudQueueClient.ConcatPdfJobQueueName);
        }

        public async Task DeleteJobFromQueueAsync(string messageId, string popReceipt)
        {
            await queueClient.DeleteMessageAsync(AzureCloudQueueClient.ConcatPdfJobQueueName, messageId, popReceipt);
        }
    }
}
