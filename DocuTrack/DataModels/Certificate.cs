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
        public int SailorID { get; set; }
        public int CertificateTypeID { get; set; }
        public string DateAcquired { get; set; }
        public string DateExpiration { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }



        public CertificateType CertificateType { get; set; }
        public Sailor Sailor { get; set; }
        public List<CertificateFile> CertificateFiles { get; set; }


        public string SailorName
        {
            get
            {
                return Sailor?.Name + " " + Sailor.Surname;
            }
        }

        public string CertificateTypeName
        {
            get
            {
                return CertificateType?.Name;
            }
        }

        public string DateOfExpiration
        {
            get
            {
                return DateTime.ParseExact(DateExpiration, "yyyy-MM-dd", null).ToString("dd-MM-yyyy");
            }
        }
    }
}
