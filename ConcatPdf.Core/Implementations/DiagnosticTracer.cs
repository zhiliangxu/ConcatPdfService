using ConcatPdf.Core.Interfaces;
using System;
using System.Diagnostics;

namespace ConcatPdf.Core.Implementations
{
    public class DiagnosticTracer : ITracer
    {
        public void Trace(TraceLevel level, string format, params object[] args)
        {
            TraceImpl(level, format, args);
        }

        public void Trace(TraceLevel level, Func<string> msgFunc)
        {
            if (msgFunc != null)
            {
                string message = msgFunc();
                TraceImpl(level, message);
            }
        }

        private void TraceImpl(TraceLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case TraceLevel.Verbose:
                case TraceLevel.Info:
                    if (args == null || args.Length == 0)
                    {
                        System.Diagnostics.Trace.TraceInformation(format);
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceInformation(format, args);
                    }
                    break;
                case TraceLevel.Warning:
                    if (args == null || args.Length == 0)
                    {
                        System.Diagnostics.Trace.TraceWarning(format);
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceWarning(format, args);
                    }
                    break;
                case TraceLevel.Error:
                    if (args == null || args.Length == 0)
                    {
                        System.Diagnostics.Trace.TraceError(format);
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceError(format, args);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
