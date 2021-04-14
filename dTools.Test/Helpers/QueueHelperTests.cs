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
    public class QueueHelperTests
    {
        private static QueueHelper<string> _queue = new QueueHelper<string>();

        [TestMethod()]
        public void QueueHelperTest()
        {
            _queue.DealAction = (x) =>
            {
                Console.WriteLine(x);
            };
        }

        [TestMethod()]
        public void QueueHelperTest1()
        {
        }

        [TestMethod()]
        public void EnqueueTest()
        {
            _queue.Enqueue("1");
        }

        [TestMethod()]
        public void DisposeTest()
        {
        }
    }
}