using Microsoft.VisualStudio.TestTools.UnitTesting;
using dTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools.Tests
{
    [TestClass()]
    public class INIHelperTests
    {
        public string INIPath { get; set; } = @"..\..\File\INIHelperTests.ini";


        [TestMethod()]
        public void Test()
        {
            //Write
            //dTools.INIHelper.Write("MySection", "MyKey", "邓振", INIPath);
            //Assert.IsTrue(1 == 1);

            ////Read
            //var read = dTools.INIHelper.Read("MySection", "MyKey", INIPath);
            //Assert.AreEqual(read, "邓振");

            ////allSections
            //var allSections = dTools.INIHelper.GetAllSectionNames(INIPath);
            //Assert.IsTrue(allSections.Count() == 1);
            //Assert.AreEqual(allSections[0], "MySection");

        }
    }
}