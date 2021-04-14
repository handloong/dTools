using Microsoft.VisualStudio.TestTools.UnitTesting;
using dTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dTools.Tests
{
    [TestClass()]
    public class EasyINITests
    {
        public string INIPath { get; set; } = @"..\..\File\EasyINITests.ini";

        [TestMethod()]
        public void Test()
        {
            //调用此方法可以自定义INI路径
            //EasyINI.SetPath(INIPath);

            //EasyINI.Write("姓名", "邓振振");
            //Assert.AreEqual(EasyINI.Read("姓名", ""), "邓振振");


            //Assert.AreEqual(EasyINI.Read("年龄默认值SSSS", 18), 18);


            //EasyINI.Write("性别男", true);
            //Assert.AreEqual(EasyINI.Read("性别男", false), true);
        }
    }
}