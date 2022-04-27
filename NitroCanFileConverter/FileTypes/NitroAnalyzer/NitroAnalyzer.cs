using DataConverter.FileTypes.Entities;
using DataConverter.Helpers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DataConverter.FileTypes.NitroAnalyzer
{
    public class NitroAnalyzer : IFileType
    {
        const int TokenCount = 12;
        int _losses = 0;
        List<Instance> instances = new List<Instance>();
        public List<Instance> Convert(string BeforePath)
        {
            List<string> Before = File.ReadAllLines(BeforePath).ToList();
            foreach (string Line in Before)
            {
              var splitline =  Line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitline.Length != TokenCount) { _losses++; continue; }
                instances.Add(AddTheInstance(splitline));
            }
            return(instances);
        }
      

        private Instance AddTheInstance(string[] splitline)
        {
            //  11ms		RX	  18F00100	8	FF  FF  FF  CD  00  FF  FF  FF
            return new Instance
            {
                time = GenerateTime(splitline[0]),
                id = GenerateID(splitline[2]),
                extended = GenerateExtended(splitline[2]), // Takes id, generates Bool
                dir = Generatdir(splitline[1]),
                bus = "0",
                data = GenerateData(splitline,Generatedlc(splitline[3])),
                dlc = Generatedlc(splitline[3])
            };
        }

        private string Generatdir(string v)
        {
            return v;
        }

        private string[] GenerateData(string[] splitline, int dlc) // use dlc here
        {
            string[] data = new string[dlc];
            for(int i = 0; i < dlc; i++)
            {
                data[i] = splitline[i+4];
            }
            return data;
        }

        private int Generatedlc(string v)
        {
            int.TryParse(v, NumberStyles.HexNumber, null, out int result);
            return result;
        }

        private bool GenerateExtended(string line)
        {
           return ConvertingHelper.isExtended(line);
        }

        private string GenerateID(string v)
        {
            return v;
        }

        private string GenerateTime(string v)
        {
            v = v.Replace("ms", String.Empty);
            return v;
        }
        public void WritetoFile(List<Instance> instList, string outputpath)
        {
            StringBuilder createText = new StringBuilder();
            foreach (Instance varinst in instList) { createText.AppendLine(ToNitro(varinst)); }
            File.WriteAllText(outputpath, createText.ToString());
            Console.WriteLine("finished NitroAnalyzer file");
        }
        public string ToNitro(Instance inst)
        {
            string datastring = "";
            for (int i = 0; i < inst.dlc; i++)
            {
                datastring += inst.data[i] + (i < (inst.dlc - 1) ? "\t" : "");
            }
            return $"{inst.time}ms \t\t{inst.dir}\t{inst.id}\t{inst.dlc}\t{datastring}";
        }

        public string Name()
        {
            return (" NitroAnalyzer.txt");
        }
    }
}
