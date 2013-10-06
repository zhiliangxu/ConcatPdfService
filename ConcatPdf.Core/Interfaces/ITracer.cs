using System;
using System.Diagnostics;

namespace ConcatPdf.Core.Interfaces
{
    public interface ITracer
    {
        void Trace(TraceLevel level, string format, params object[] args);

        void Trace(TraceLevel level, Func<string> msgFunc);
    }
}
