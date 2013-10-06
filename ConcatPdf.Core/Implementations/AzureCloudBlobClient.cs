using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Implementations
{
    internal class AzureCloudBlobClient
    {
        public const string PdfBlobContainerName = "outpdf";

        private CloudBlobClient blobClient;

        public AzureCloudBlobClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task UploadFromStreamAsync(Stream source, string fileName)
        {
            ICloudBlob blob = blobClient.GetContainerReference(PdfBlobContainerName).GetBlockBlobReference(fileName);
            await blob.UploadFromStreamAsync(source);
        }

        public async Task<Stream> OpenWriteBlobStream(string fileName)
        {
            CloudBlockBlob blob = blobClient.GetContainerReference(PdfBlobContainerName).GetBlockBlobReference(fileName);
            return await blob.OpenWriteAsync();
        }

        public string GetBlobUri(string fileName)
        {
            Uri containerUri = blobClient.GetContainerReference(PdfBlobContainerName).Uri;
            return containerUri + "/" + fileName;
        }
    }
}
