using DataConverter.FileTypes.Entities;
using DataConverter.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.BusWatchAnalyzer
{
    public class BusWatchAnalyzer : IFileType
    {
        // remove token system, and make it so any line that starts with number is our data
        const int TokenCount = 14;
        int _losses = 0;
        List<Instance> instances = new List<Instance>();
        public List<Instance> Convert(string BeforePath)
        {
            List<string> Before = File.ReadAllLines(BeforePath).ToList();
            foreach (string Line in Before)
            {
                var splitline = Line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (CheckIfLine(splitline[0]) == false) { _losses++; continue; }
                instances.Add(AddTheInstance(splitline));
            }
            return instances;
        }


        private Instance AddTheInstance(string[] splitline)
        {
            // 0.000000 1  18FF3D23x            Rx   d 8 FF FF FF FF FF FF FF 16

            return new Instance
            {
                time = GenerateTime(splitline[0]),
                id = GenerateID(splitline[2]),
                extended = GenerateExtended(splitline[2]), // Takes id, generates bool
                dir = Generatdir(splitline[3]),
                bus = "1",
                data = GenerateData(splitline, Generatedlc(splitline[5])),
                dlc = Generatedlc(splitline[5])
            };
        }

        private bool CheckIfLine(string v)
        {
            var charsToRemove = new string[] { "." };
            foreach (var c in charsToRemove)
            {
                v = v.Replace(c, string.Empty);
            }
            if (int.TryParse(v, NumberStyles.HexNumber, null, out int result))
            {
                return true;
            }
            else return false;
        }
        private string Generatdir(string v)
        {
            return v;
        }

        private string[] GenerateData(string[] splitline, int dlc) // use dlc here
        {
            string[] data = new string[dlc];
            for (int i = 0; i < dlc; i++)
            {
                data[i] = splitline[i + 6];
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
            var charsToRemove = new string[] { "x" };
            foreach (var c in charsToRemove)
            {
                line = line.Replace(c, string.Empty);
            }
            return ConvertingHelper.isExtended(line);
        }

        private string GenerateID(string v)
        {
            var charsToRemove = new string[] { "x" };
            foreach (var c in charsToRemove)
            {
                v = v.Replace(c, string.Empty);
            }


            return v;
        }

        private string GenerateTime(string v)
        {
            var charsToRemove = new string[] {"."};
            foreach (var c in charsToRemove)
            {
                v = v.Replace(c, string.Empty);
            }
            return v;
        }
         public void WritetoFile(List<Instance> instList, string outputpath)
        {
            StringBuilder createText = new StringBuilder();
            foreach (Instance varinst in instList) { createText.AppendLine(ToBus(varinst)); }
            File.WriteAllText(outputpath, createText.ToString());
            Console.WriteLine("finished BusWatch file");
        }
         public string ToBus(Instance inst) 
        {
            string datastring = "";
            for (int i = 0; i < inst.dlc; i++)
            {
                datastring += inst.data[i] + (i < (inst.dlc - 1) ? " " : "");
            }

            string timestring = inst.time;
            if ( timestring.Length > 2)
                { timestring.Insert(2, "."); }
            return $"\t{timestring}\t\t{inst.bus}\t{inst.id}\t\t\t{inst.dir}\td\t{inst.dlc}\t{datastring}";
        }

        public string Name()
        {
            return (" BusWatchAnalyzer.txt");
        }
    }
}
