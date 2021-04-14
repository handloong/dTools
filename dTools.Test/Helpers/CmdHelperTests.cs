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
    public class CmdHelperTests
    {
        [TestMethod()]
        public void RunTest()
        {
            //var res = CmdHelper.Run("ipconfig");
            //Assert.IsTrue(res.Contains("Microsoft"));

            //            var s = "\"";
            //            var str = $@"
            //del /f /s /q  %systemdrive%\*.tmp
            //del /f /s /q  %systemdrive%\*._mp
            //del /f /s /q  %systemdrive%\*.log
            //del /f /s /q  %systemdrive%\*.gid
            //del /f /s /q  %systemdrive%\*.chk
            //del /f /s /q  %systemdrive%\*.old
            //del /f /s /q  %systemdrive%\recycled\*.*
            //del /f /s /q  %windir%\*.bak
            //del /f /s /q  %windir%\prefetch\*.*
            //rd /s /q %windir%\temp & md  %windir%\temp
            //del /f /q  %userprofile%\cookies\*.*
            //del /f /q  %userprofile%\recent\*.*
            //del /f /s /q  {s}%userprofile%\Local Settings\Temporary Internet Files\*.*{s}
            //del /f /s /q  {s}%userprofile%\Local Settings\Temp\*.*{s}
            //del /f /s /q  {s}%userprofile%\recent\*.*{s}
            //";
            //CmdHelper.Run(str);
        }
    }
}