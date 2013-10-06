using System.Threading.Tasks;
using WebBackgrounder;

namespace ConcatPdf.Web.Worker
{
    public class ConcatPdfJobCoordinator : IJobCoordinator
    {
        public void Dispose()
        {
        }

        public Task GetWork(IJob job)
        {
            return job.Execute();
        }
    }
}
