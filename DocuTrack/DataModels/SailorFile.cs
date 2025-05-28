using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class SailorFile
    {
        public int ID { get; set; }

        public int SailorID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

    }
}
