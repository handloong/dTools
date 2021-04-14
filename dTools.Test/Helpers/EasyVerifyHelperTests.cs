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
    public class EasyVerifyHelperTests
    {
        [TestMethod()]
        public void VTest()
        {
            //User user = new User() { Id = "" };
            //EasyVerifyHelper.Trythrow(user);
        }
    }

    public class User
    {
        [DSV(NotNull = true)]
        public string Id { get; set; }
    }
}