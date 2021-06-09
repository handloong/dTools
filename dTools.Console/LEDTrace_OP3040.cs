using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dTools;

namespace Lux.KNS.CSV
{
    [CSVHeader(0)]
    public class LEDTrace_OP3040
    {
        [CSVColumn("BARCODE")]
        public string Barcode { get; set; }

        /// <summary>
        /// 取自于文件
        /// </summary>
        /// 
        public DateTime StartTime { get; set; }

        [CSVColumn("STATIONTYPE")]
        public string StationType { get; set; }

        [CSVColumn("CompName")]
        public string Comp_Name { get; set; }

        [CSVColumn("COMPTYPE")]
        public string Part_Type { get; set; }

        [CSVColumn("_SHIFTX")]
        public string ShiftX { get; set; }

        [CSVColumn("_SHIFTY")]
        public string ShiftY { get; set; }

        [CSVColumn("_THETA")]
        public string Theta { get; set; }

        [CSVColumn("ERRORCODE")]
        public string ErrorCode { get; set; }

        [CSVColumn("GRAY_LEVEL")]
        public string GRAY_LEVEL { get; set; }

        [CSVColumn("POSX")]
        public string PosX { get; set; }

        [CSVColumn("POSY")]
        public string PosY { get; set; }

        [CSVColumn("STATUS")]
        public string Status { get; set; }
    }
}
