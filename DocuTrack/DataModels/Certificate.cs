using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace DocuTrack.DataModels
{
    public class Certificate
    {
        public int ID { get; set; }
        public int SailorID { get; set; }
        public int CertificateTypeID { get; set; }
        public string DateAcquired { get; set; }
        public string DateExpiration { get; set; }

        public DateTime? DateAcquiredDate { get; set; } = DateTime.Today;
        public DateTime? DateExpirationDate { get; set; } = DateTime.Today.AddYears(1);
        public string Place { get; set; }

        public ComboBoxItem PlaceComboBox { get; set; }
        public string Name { get; set; }



        public CertificateType CertificateType { get; set; }
        public Sailor Sailor { get; set; }
        public List<Certificate> Versions { get; set; } = new List<Certificate>();
        public List<CertificateFile> CertificateFiles { get; set; }

        public bool isOldVersion { get; set; } = false;

        public bool isNew
        {
            get { return !isOldVersion; }
        }


        public string SailorName
        {
            get
            {
                return Sailor?.Name + " " + Sailor?.Surname;
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

        public string NameWithDateSpan
        {
            get
            {
                return Name + " (" + DateTime.ParseExact(DateAcquired, "yyyy-MM-dd", null).ToString("dd-MM-yyyy") + " - " + DateTime.ParseExact(DateExpiration, "yyyy-MM-dd", null).ToString("dd-MM-yyyy") + ")";
            }
        }
    }
}
