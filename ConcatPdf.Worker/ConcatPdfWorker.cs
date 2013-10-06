using ConcatPdf.Core.Interfaces;
using ConcatPdf.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConcatPdf.Worker
{
    public class ConcatPdfWorker : IBackendJob
    {
        private const int maxDegreeOfParallelism = 5;
        private ITracer _tracer;
        private IJobRepository _repository;
        private IConcatPdf _concatPdf;
        private IPdfStorage _pdfStorage;
        private SemaphoreSlim _semaphore;
        private TransformBlock<Tuple<string, string, string>, Tuple<ConcatPdfJob, string, string>> _getJobDetailsBlock;
        private ActionBlock<Tuple<ConcatPdfJob, string, string>> _concatPdfBlock;

        public ConcatPdfWorker(ITracer tracer, IJobRepository repository, IConcatPdf concatPdf, IPdfStorage pdfStorage)
        {
            this._tracer = tracer;
            this._repository = repository;
            this._concatPdf = concatPdf;
            this._pdfStorage = pdfStorage;
            this._semaphore = new SemaphoreSlim(maxDegreeOfParallelism * 2);

            ExecutionDataflowBlockOptions executionDataflowBlockOptions = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                };

            _getJobDetailsBlock = new TransformBlock<Tuple<string, string, string>, Tuple<ConcatPdfJob, string, string>>(async jobQueueItem =>
                {
                    try
                    {
                        string jobId = jobQueueItem.Item1;
                        ConcatPdfJob job = await this._repository.GetJobAsync(jobId);

                        if (job.Status == "Processing" && (DateTime.UtcNow - job.ProcessedDate < TimeSpan.FromMinutes(30)))
                        {
                            return null;
                        }

                        job.Status = "Processing";
                        job.ProcessedDate = DateTime.UtcNow;
                        job.ProcessingAttempts++;
                        await _repository.UpdateJobAsync(job);

                        return Tuple.Create(job, jobQueueItem.Item2, jobQueueItem.Item3);
                    }
                    catch (Exception ex)
                    {
                        this._tracer.Trace(System.Diagnostics.TraceLevel.Error, "Exception in _getJobDetailsBlock: {0}", ex);
                        return null;
                    }
                },
                executionDataflowBlockOptions);

            _concatPdfBlock = new ActionBlock<Tuple<ConcatPdfJob, string, string>>(async jobQueueItem =>
                {
                    try
                    {
                        if (jobQueueItem == null)
                        {
                            return;
                        }

                        ConcatPdfJob job = jobQueueItem.Item1;
                        string messageId = jobQueueItem.Item2;
                        string popReceipt = jobQueueItem.Item3;

                        if (job.Status == "Completed" || job.ProcessingAttempts > 5)
                        {
                            await this._repository.DeleteJobFromQueueAsync(messageId, popReceipt);
                        }

                        using (Stream memoryStream = await _pdfStorage.OpenWriteBlobStream(job.OutputFile))
                        {
                            this._concatPdf.ConcatPdf(job.InputUris, memoryStream);
                        }

                        job.OutputUri = _pdfStorage.GetBlobUri(job.OutputFile);
                        job.Status = "Completed";
                        await _repository.UpdateJobAsync(job);

                        await this._repository.DeleteJobFromQueueAsync(messageId, popReceipt);

                        this._tracer.Trace(System.Diagnostics.TraceLevel.Warning, "Successfully processed job: {0}", job.Id);
                    }
                    catch (Exception ex)
                    {
                        this._tracer.Trace(System.Diagnostics.TraceLevel.Error, "Exception in _concatPdfBlock: {0}", ex);
                    }
                    finally
                    {
                        this._semaphore.Release();
                    }
                },
                executionDataflowBlockOptions);

            _getJobDetailsBlock.LinkTo(_concatPdfBlock);

            _getJobDetailsBlock.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    ((IDataflowBlock)_concatPdfBlock).Fault(t.Exception);
                }
                else
                {
                    _concatPdfBlock.Complete();
                }
            });
        }

        public async Task Run()
        {
            while (true)
            {
                var jobQueueItems = await _repository.GetJobsFromQueueAsync();
                
                if (!jobQueueItems.Any())
                {
                    break;
                }

                await _semaphore.WaitAsync();

                foreach (var item in jobQueueItems)
                {
                    await this._semaphore.WaitAsync();
                    this._getJobDetailsBlock.Post(item);
                }
            }

            this._getJobDetailsBlock.Complete();
            await this._getJobDetailsBlock.Completion;
        }
    }
}
