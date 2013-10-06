using ConcatPdf.Core.Interfaces;
using System.Collections.Generic;
using System.IO;
using Ujihara.PDF;

namespace ConcatPdf.Core.Implementations
{
    public class PdfConcatenatorWrapper : IConcatPdf
    {
        public void ConcatPdf(IEnumerable<string> uris, Stream outStream)
        {
            using (PdfConcatenator con = new PdfConcatenator(outStream, null, 0))
            {
                foreach (var uri in uris)
                {
                    con.Append(uri, null, new PdfConcatenatorOption());
                }
                con.Close();
            }
        }
    }
}
