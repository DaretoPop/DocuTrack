using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class RequestFile : IDocument
    {
        public int ID { get; set; }

        public int CertificateTypeID {get; set; }

        public RequestTypeEnum RequestType { get; set; }
        
        public string FilePath { get; set; }
    }
}
