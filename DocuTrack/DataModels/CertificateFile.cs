using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class CertificateFile
    {
        public int ID { get; set; }

        public int CertificateID { get; set; }
        public int SailorGID { get; set; }

        public bool PrimaryFile { get; set; }

        public string FilePath { get; set; }

    }
}
