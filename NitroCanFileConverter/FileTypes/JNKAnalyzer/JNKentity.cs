using DataConverter.FileTypes.Entities;
using DataConverter.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.FileTypes.JNKAnalyzer
{
    internal class JNKentity
    {
        public string canId { get; set; }
        public int elapseTime { get; set; }
        public string dlc { get; set; }
        public string rawData { get; set; }

    
        public Instance toInstance()
        {
            string[] split = new string[rawData.Length / 2 + (rawData.Length % 2 == 0 ? 0 : 1)];



            for (int i = 0; i < split.Length; i++)

            {

                split[i] = rawData.Substring(i * 2, i * 2 + 2 > rawData.Length ? 1 : 2);

            }
            return new Instance
            {
                data = split,
                id = canId,
                bus = "1",
                dir = "Rx",
                dlc = (Generatedlc(dlc)), // make to int
                extended = GenerateExtended(canId), //>7ff
                time = GenerateTime(elapseTime),
            };
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
        
        private string GenerateTime(int time)
        {
            return time.ToString();
        }
    }
}
