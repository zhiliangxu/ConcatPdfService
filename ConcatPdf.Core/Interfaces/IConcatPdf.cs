using System.Collections.Generic;
using System.IO;

namespace ConcatPdf.Core.Interfaces
{
    public interface IConcatPdf
    {
        void ConcatPdf(IEnumerable<string> uris, Stream outStream);
    }
}
