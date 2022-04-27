using DataConverter.FileTypes.Entities;
using DataConverter.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.SavvyCanConvertor
{
      public class SavvyCanConvertor : IFileType // if broken put class back to static then remove ifile type stuff
        {
        const int TokenCount = 14;
        int _losses = 0;
        List<Instance> instances = new List<Instance>();
       
        // 11,  18F00100,TRUE,RX,0,8,FF,FF,FF,CD,0,FF,FF,FF
        public List<Instance> Convert(string BeforePath)
        {
            List<string> Before = File.ReadAllLines(BeforePath).ToList();
            foreach (string Line in Before)
            {
        
                var splitline = Line.Split(new char[] { ' ', '\t' ,','}, StringSplitOptions.RemoveEmptyEntries);
                if (splitline.Length != TokenCount) { _losses++; continue; }
                instances.Add(AddTheInstance(splitline));
            }
            return instances;
        }

        private Instance AddTheInstance(string[] splitline)
        {       
            //  11,  18F00100,TRUE,RX,0,8,FF,FF,FF,CD,0,FF,FF,FF
            return new Instance
            {
                time = GenerateTime(splitline[0]),
                id = GenerateID(splitline[1]),
                extended = GenerateExtended(splitline[2]), // Takes id, generates Bool
                dir = Generatdir(splitline[3]),
                bus = "0",
                data = GenerateData(splitline, Generatedlc(splitline[5])),
                dlc = Generatedlc(splitline[5])
            };
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
        //TODO: Fix the generations for Savvycan
        private int Generatedlc(string v)
        {
            int.TryParse(v, NumberStyles.HexNumber, null, out int result);
            return result;
        }

        private bool GenerateExtended(string line)
        {
            return line == "TRUE";
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
            createText.AppendLine("Time Stamp,ID,Extended,Dir,Bus,LEN,D1,D2,D3,D4,D5,D6,D7,D8");
            foreach (Instance varinst in instList) { createText.AppendLine(ToCan(varinst)); }
            File.WriteAllText(outputpath, createText.ToString());
            Console.WriteLine("finished SavvyCan file");
        }
        public string ToCan(Instance inst)
        {
            string datastring = "";
            for(int i = 0; i < inst.dlc; i++)
            {
                datastring += inst.data[i] + (i < (inst.dlc-1)?"," :"");
            }
            return $"{inst.time},  {inst.id},{inst.extended},{inst.dir},{inst.bus},{inst.dlc},{datastring}"; // write for each from length to id
        }
    }
}
