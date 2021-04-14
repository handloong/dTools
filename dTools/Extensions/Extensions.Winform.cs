using System;
using System.Drawing;
using System.Windows.Forms;

namespace dTools.Extensions
{
    /// <summary>
    /// Winform一些扩展
    /// </summary>
    public static class ExtensionsWinform
    {
        /// <summary>
        /// 设置控件的Text值并加上时间(支持跨线程访问)
        /// <para>实现如下:</para>
        /// <list type="string" >
        ///  <item> @this.Text = $"[{DateTime.Now}]{msg}";</item>
        /// </list>
        /// </summary>
        /// <param name="this"></param>
        /// <param name="msg"></param>
        public static void SetText(this Control @this, string msg)
        {
            if (@this.InvokeRequired)
                @this.BeginInvoke(new Action(() => { @this.Text = $"[{DateTime.Now}]{msg}"; }));
            else
                @this.Text = $"[{DateTime.Now}]{msg}";
        }

        /// <summary>
        /// 向RichTextBox添加文字(支持跨线程访问)
        /// </summary>
        /// <param name="this">RichTextBox</param>
        /// <param name="msg">信息</param>
        /// <param name="color">颜色</param>
        /// <param name="appendTime">是否显示时间</param>
        /// <param name="timeFormat">时间格式</param>
        /// <param name="clearCount">RichTextBox长度多少清空</param>
        public static void Write(this System.Windows.Forms.RichTextBox @this,
            string msg,
            Color color,
            bool appendTime = true,
            string timeFormat = "yyyy-MM-dd HH:mm:ss",
            int clearCount = 4000)
        {
            string displayMsg;
            if (appendTime)
                displayMsg = $@"{DateTime.Now.ToString(timeFormat)}:{msg}{Environment.NewLine}";
            else
                displayMsg = $@"{msg}{Environment.NewLine}";
            if (@this.InvokeRequired)
            {
                @this.BeginInvoke(new Action(() =>
                {
                    Set(@this, color, displayMsg, clearCount);
                }));
            }
            else
            {
                Set(@this, color, displayMsg, clearCount);
            }

            void Set(RichTextBox a, Color b, string c, int d)
            {
                if (a.Text.Length > d)
                    a.Text = "";
                a.Select(a.Text.Length, 0);
                a.Focus();
                a.SelectionColor = b;
                a.AppendText(c);
            }
        }

        /// <summary>
        /// 设置FormBorderStyle为FixedSingle(不支持跨线程访问)
        /// <para>实现如下:</para>
        /// <list type="string" >
        ///  <item>form.ShowIcon = false;</item>
        ///  <item>form.StartPosition = FormStartPosition.CenterScreen;</item>
        ///  <item>form.MaximizeBox = false;</item>
        ///  <item>form.MinimizeBox = false;</item>
        ///  <item>form.FormBorderStyle = FormBorderStyle.FixedSingle;</item>
        ///  <item>form.WindowState = FormWindowState.Normal;</item>
        ///  <item>form.StartPosition = FormStartPosition.CenterScreen;</item>
        /// </list>
        /// <param name="form"></param>
        /// </summary>
        public static void FixedSingle(this Form form)
        {
            form.ShowIcon = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
