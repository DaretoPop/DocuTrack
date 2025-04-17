using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuTrack.DataModels
{
    public interface IDocument 
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
    }
}
