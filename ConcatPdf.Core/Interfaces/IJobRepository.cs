using ConcatPdf.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Interfaces
{
    public interface IJobRepository
    {
        Task CreateJobAsync(ConcatPdfJob job);

        Task UpdateJobAsync(ConcatPdfJob job);

        Task<IEnumerable<Tuple<string, string, string>>> GetJobsFromQueueAsync();

        Task EnqueueJobAsync(string jobId);

        Task DeleteJobFromQueueAsync(string messageId, string popReceipt);

        Task<ConcatPdfJob> GetJobAsync(string id);
    }
}
