using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dTools;
using Lux.KNS.CSV;

namespace dTools.Console
{
    class Program
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int Age { get; set; }
        public int Money { get; set; }
        public bool Open { get; set; }
        public decimal AA { get; set; }
        static void Main(string[] args)
        {
            var csv = @"C:\Users\H12727182\Desktop\Serin (2)\AOI OP30 40\40.csv";
            var csv1 = @"C:\Users\H12727182\Desktop\Serin (2)\AOI OP30 40\41.csv";

            
            var move = @"C:\Users\H12727182\Desktop\Serin (2)\Cyber OP10 50 60\新建文本文档.txt";
            var x1608 = CSVHelper.Paser<X1608>(File.ReadAllLines(csv1));
        }
    }

    [CSVHeader(0)]
    class X1608
    {
        [CSVColumn("BoardNo")]
        public string BoardNo { get; set; }

        [CSVColumn("BoardNo1")]
        public string BoardNo1 { get; set; }

    }
}
