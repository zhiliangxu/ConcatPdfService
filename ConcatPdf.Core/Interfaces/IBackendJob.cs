using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcatPdf.Core.Interfaces
{
    public interface IBackendJob
    {
        Task Run();
    }
}
