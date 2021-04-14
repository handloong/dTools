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
    public class RetryHelperTests
    {
        private int GetNotSupportedValues(bool throwEx)
        {
            if (throwEx)
            {
                throw new NotSupportedException("NO");
            }
            return 10086;
        }
        private int GetMissingFieldValues(bool throwEx)
        {
            if (throwEx)
            {
                throw new MissingFieldException("NO");
            }
            return 10086;
        }

        [TestMethod()]
        public void RetryOnAnyTest()
        {
            var @this = RetryHelper.RetryOnAny<int>(times: 2, action: () =>
             {
                 return GetNotSupportedValues(false);
             });
            Assert.AreEqual(@this, 10086);

            //throw ,return defalut value ,int => 0
            var @this1 = RetryHelper.RetryOnAny<int>(times: 1, action: () =>
            {
                return GetNotSupportedValues(true);
            });
            Assert.AreEqual(@this1, 0);
        }

        [TestMethod()]
        public void RetryOnAnyTest1()
        {
            //no ex
            var @thisInt = 0;
            var times = 2;
            var @this = RetryHelper.RetryOnAny<int>(times: times, action: () =>
            {
                return GetNotSupportedValues(false);
            }, (i, e) =>
            {
                @thisInt += i;
            });
            Assert.AreEqual(@this, 10086);
            Assert.AreEqual(@thisInt, 0);

            //ex
            @this = 0;
            @thisInt = 0;
            @this = RetryHelper.RetryOnAny<int>(times: times, action: () =>
            {
                return GetNotSupportedValues(true);
            }, (i, e) =>
            {
                @thisInt += i;
            });
            Assert.AreEqual(@this, 0);
            //1 + 2 = 3
            Assert.AreEqual(@thisInt, 3);
        }

        [TestMethod()]
        public void RetryOnExceptionTest()
        {
            var times = 2;
            var @thisInt = 0;
            //no ex
            var @this = RetryHelper.RetryOnException<int, NotSupportedException>(times, (a) =>
            {
                return GetNotSupportedValues(false);
            }, (i, e) =>
            {

            });
            Assert.AreEqual(@this, 10086);

            // ex
            @this = 0;
            @thisInt = 0;
            @this = RetryHelper.RetryOnException<int, MissingFieldException>(times, (a) =>
           {
               return GetMissingFieldValues(true);
           }, (i, e) =>
           {
               @thisInt += i;
           });
            Assert.AreEqual(@this, 0);
            Assert.AreEqual(@thisInt, 3);
            // ex ,other ex
            @this = 0;
            @thisInt = 0;
            @this = RetryHelper.RetryOnException<int, NotSupportedException>(times, (a) =>
            {
                return GetMissingFieldValues(true);
            }, (i, e) =>
            {
                @thisInt += i;
            });
            Assert.AreEqual(@this, 0);
            Assert.AreEqual(@thisInt, 0);
        }
    }
}