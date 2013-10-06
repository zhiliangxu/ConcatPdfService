using ConcatPdf.Core.Implementations;
using ConcatPdf.Core.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using WebActivatorEx;
using WebBackgrounder;

[assembly: PostApplicationStartMethod(typeof(ConcatPdf.Web.Worker.BackgroundWorkerActivator), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(ConcatPdf.Web.Worker.BackgroundWorkerActivator), "Stop")]

namespace ConcatPdf.Web.Worker
{
    public static class BackgroundWorkerActivator
    {
        private static JobManager _jobManager;
        private static ITracer _tracer;

        static BackgroundWorkerActivator()
        {
            _tracer = ContainerProvider.Current.Resolve<ITracer>();
        }

        public static void PostStart()
        {
            _tracer.Trace(TraceLevel.Warning, "Background worker activator started.");

            IJob[] jobs = new IJob[]
            {
                new ConcatPdfBackendJob(TimeSpan.FromMinutes(1.0), TimeSpan.MaxValue)
            };

            _jobManager = new JobManager(jobs, new JobHost())
            {
                RestartSchedulerOnFailure = true
            };

            _jobManager.Fail(e => _tracer.Trace(TraceLevel.Error, "Job manager failed: {0}", e));
            _jobManager.Start();
        }

        private static void Stop()
        {
            _tracer.Trace(TraceLevel.Warning, "Background worker activator stopped.");
            _jobManager.Dispose();
        }
    }
}
