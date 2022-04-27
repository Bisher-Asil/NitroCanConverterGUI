using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.Entities
{
    public class Instance
    {
        string _id;
        public string id { 
            get { return _id; }
            set {
                if (value.Length < 8) { _id = value.PadLeft(8,(char)0x20) ; }
                else { _id = value; }
            } 
        }
        public string time { get; set; }
        public string dir { get; set; }
        public string bus { get; set; }
        public string[] data { get; set; }
        public bool extended { get; set; }
        public int dlc { get; set; }

        
    }
}
