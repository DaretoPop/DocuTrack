using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class CertificateType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<RequestFile> Documents { get; set; }
    }
}
