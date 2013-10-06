using ConcatPdf.Core.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Implementations
{
    public class AzureBlobPdfStorage : IPdfStorage
    {
        AzureCloudBlobClient blobClient = new AzureCloudBlobClient();

        public async Task UploadFromStreamAsync(Stream source, string fileName)
        {
            await blobClient.UploadFromStreamAsync(source, fileName);
        }

        public async Task<Stream> OpenWriteBlobStream(string fileName)
        {
            return await blobClient.OpenWriteBlobStream(fileName);
        }

        public string GetBlobUri(string fileName)
        {
            return blobClient.GetBlobUri(fileName);
        }
    }
}
