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
    public class ExtensionsValidateTests
    {
        [TestMethod()]
        public void IsEmptyTest()
        {
            var entity = new ExtensionsValidateTests();
            entity = null;
            Assert.AreEqual(entity.IsEmpty(), true);
        }
    }
}