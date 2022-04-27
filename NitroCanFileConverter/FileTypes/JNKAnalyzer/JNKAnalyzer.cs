using DataConverter.FileTypes.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.JNKAnalyzer
{
    public class JNKAnalyzer : IFileType
    {
        const int TokenCount = 12;
        int _losses = 0;
        List<Instance> instances = new List<Instance>();
        public List<Instance> Convert(string BeforePath)
        {
            // JNKentitiy entity = JsonConvert.DeserializeObject<entity>(line);

            //you have List<instanse> dfsdffs
            //fsfsf.add(entity.toInstance());
            List<string> Before = File.ReadAllLines(BeforePath).ToList();
            foreach (string Line in Before)
            {
                try
                {
                    var lineAfterSlash = Line.Replace("\"", "'");
                    JNKentity entity = JsonConvert.DeserializeObject<JNKentity>(Line);
                    if (entity == null) continue;
                    instances.Add(entity.toInstance());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return instances;
        }
        public void WritetoFile(List<Instance> instList, string outputpath)
        {
            StringBuilder createText = new StringBuilder();
            foreach (Instance varinst in instList) { createText.AppendLine(ToJNK(varinst)); }
            File.WriteAllText(outputpath, createText.ToString());
            Console.WriteLine("finished JNK file");
        }

        public string ToJNK(Instance inst) //TODO: make this to JNK
        {
            //{"canId":"186","elapseTime":60011,"dlc":"7","rawData":"0000320412BE9A"}
            int.TryParse(inst.time, NumberStyles.HexNumber, null, out int result);

            string datastring = string.Join("", inst.data);

            var Serial = new JNKentity { canId = inst.id, dlc = inst.dlc.ToString(), elapseTime = result, rawData = datastring };

            string jsonString = JsonConvert.SerializeObject(Serial);

            // TODO: check if worked

            return $"{jsonString}";
        }

       
    }
 }
