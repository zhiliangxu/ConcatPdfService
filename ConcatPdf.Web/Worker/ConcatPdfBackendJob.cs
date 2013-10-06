using ConcatPdf.Core.Implementations;
using ConcatPdf.Core.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using WebBackgrounder;

namespace ConcatPdf.Web.Worker
{
    public class ConcatPdfBackendJob : Job
    {
        public ConcatPdfBackendJob(TimeSpan frequence, TimeSpan timeout)
            : base("ConcatPdf", frequence, timeout)
        {
        }

        public override Task Execute()
        {
            IBackendJob worker = ContainerProvider.Current.Resolve<IBackendJob>();
            return new Task(() => worker.Run().Wait());
        }
    }
}