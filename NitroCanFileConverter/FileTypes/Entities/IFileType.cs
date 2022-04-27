using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.Entities
{
    public interface IFileType
    {
        List<Instance> Convert(string BeforePath);
        void WritetoFile(List<Instance> instList, string outputpath);
    }
}
