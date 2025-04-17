using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public class Sailor
    {
        public int GID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRefresh { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}
