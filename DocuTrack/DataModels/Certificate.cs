using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class Certificate
    {
        public int ID { get; set; }
        public int SailorGID { get; set; }
        public int CertificateTypeID { get; set; }
        public string DateAcquired { get; set; }
        public string DateExpiration { get; set; }
        public string Place { get; set; }



        

        public List<CertificateFile> CertificateFiles { get; set; }
    }
}
