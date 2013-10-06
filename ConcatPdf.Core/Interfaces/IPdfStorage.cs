using System.IO;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Interfaces
{
    public interface IPdfStorage
    {
        Task UploadFromStreamAsync(Stream source, string fileName);

        Task<Stream> OpenWriteBlobStream(string fileName);

        string GetBlobUri(string fileName);
    }
}
