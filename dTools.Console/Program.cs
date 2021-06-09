using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dTools;


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
            var csv = @"C:\Users\H12727182\Desktop\Serin (2)\AOI OP30 40\X1608_0DVT_SS13_639-14580_OP40_V001_4001_IIII_AH0AQ0541N3CA_20201202_212512.csv";
            var csv1 = @"C:\Users\H12727182\Desktop\Serin (2)\Cyber OP10 50 60\Recipe name_OP50_V001_CY01.LotNO.barcode.date.time.R.csv";
            var move = @"C:\Users\H12727182\Desktop\Serin (2)\Cyber OP10 50 60\新建文本文档.txt";
            FileHelper.MoveFile(new FileInfo(move));
            var x1608 = CSVHelper.Paser<X1608>(File.ReadAllLines(csv));
        }
    }

    [CSVHeader(10)]
    class X1608
    {
        [CSVColumn("BoardNo")]
        public string BoardNo { get; set; }

        [CSVColumn("POS X-")]
        public int POSTX { get; set; }


        [CSVColumn("CompType")]
        public string 数据库 { get; set; }

        [CSVColumn("DzzDate")]
        public DateTime? DzzDate { get; set; }
    }
}
