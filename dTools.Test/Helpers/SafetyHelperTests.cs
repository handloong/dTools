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
    public class SafetyHelperTests
    {
        [TestMethod()]
        public void Test()
        {
            var @this = dTools.SafetyHelper.DescEncrypt("邓振振");
            Assert.AreEqual("邓振振", dTools.SafetyHelper.DescCrypt(@this));
        }

    }
}