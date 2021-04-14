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
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void ToPinyinTest()
        {
            var @this = "邓振振".ToPinyin();
            Assert.AreEqual(@this, "dengzhenzhen");
        }
    }
}