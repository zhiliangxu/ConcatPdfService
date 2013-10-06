using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Implementations
{
    public class AzureCloudQueueClient
    {
        public const string ConcatPdfJobQueueName = "concatpdfjobs";

        private CloudQueueClient queueClient;

        public AzureCloudQueueClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            queueClient = storageAccount.CreateCloudQueueClient();
        }

        public async Task EnqueueAsync(string queueName, string jobId)
        {
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            CloudQueueMessage cqm = new CloudQueueMessage(jobId);
            await queue.AddMessageAsync(cqm);
        }

        public async Task<IEnumerable<Tuple<string, string, string>>> GetMessagesAsync(string queueName, int messageCount = 5)
        {
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            IEnumerable<CloudQueueMessage> messages = await queue.GetMessagesAsync(messageCount, TimeSpan.FromMinutes(20), null, null);
            return messages.Select(m => Tuple.Create(m.AsString, m.Id, m.PopReceipt));
        }

        public async Task DeleteMessageAsync(string queueName, string messageId, string popReceipt)
        {
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            await queue.DeleteMessageAsync(messageId, popReceipt, null, null);
        }
    }
}
