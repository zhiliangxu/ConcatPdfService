using ConcatPdf.Core.Interfaces;
using ConcatPdf.Core.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConcatPdf.Web.Controllers
{
    [WebApiExceptionFilter]
    public class ConcatPdfController : ApiController
    {
        private ITracer _tracer;
        private IJsonSerializer _serializer;
        private IJobRepository _repository;

        public ConcatPdfController(ITracer tracer, IJsonSerializer serializer, IJobRepository repository)
        {
            this._tracer = tracer;
            this._serializer = serializer;
            this._repository = repository;
        }

        /// <summary>
        /// Get the Concat PDF job by <paramref name="id">ID</paramref>.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        /// <returns>The Concat PDF job whose ID is <paramref name="id">ID</paramref>.</returns>
        public async Task<HttpResponseMessage> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            ConcatPdfJob job = await _repository.GetJobAsync(id);

            _tracer.Trace(TraceLevel.Warning, () =>
                string.Format("Get posted: {0}", _serializer.Serialize(job)));

            if (job == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return this.Request.CreateResponse<ConcatPdfJob>(
                HttpStatusCode.OK, job);
        }

        /// <summary>
        /// Post a new Concat PDF job.
        /// </summary>
        /// <param name="job">The Concat PDF job to be posted.</param>
        /// <returns>The posted Concat PDF job.</returns>
        public async Task<HttpResponseMessage> Post([FromBody]ConcatPdfJob job)
        {
            if (job == null)
            {
                throw new ArgumentNullException("job");
            }

            if (job.InputUris == null)
            {
                throw new ArgumentNullException("job.InputUris");
            }

            if (job.InputUris.Count == 0)
            {
                throw new ArgumentException("job.InputUris is empty.");
            }

            job.Id = Guid.NewGuid().ToString("N");
            job.SubmittedDate = DateTime.UtcNow;
            job.OutputFile = string.Format("{0}.pdf", job.Id);
            job.Status = "Scheduled";
            job.ProcessedDate = DateTime.FromFileTimeUtc(0L);

            _tracer.Trace(TraceLevel.Warning, () =>
                string.Format("ConcatPdfJob posted: {0}", this._serializer.Serialize(job)));

            await _repository.CreateJobAsync(job);
            await _repository.EnqueueJobAsync(job.Id);
            return this.Request.CreateResponse(HttpStatusCode.Created, job);
        }
    }
}