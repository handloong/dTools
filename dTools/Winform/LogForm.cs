using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dTools
{
    /// <summary>
    /// winform 的 LogForm
    /// </summary>
    public partial class LogForm : Form
    {
        /// <summary>
        /// 构造
        /// </summary>
        public LogForm()
        {
            InitializeComponent();

            var screen = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));
            var x = screen.WorkingArea.X + screen.WorkingArea.Width - this.Width;
            var y = screen.WorkingArea.Y + screen.WorkingArea.Height - this.Height;
            this.Location = new Point(x, y);

            Instance = this;
            this.FormClosed += LogForm_FormClosed;
        }

        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }

        private static LogForm _instance;
        /// <summary>
        /// 实例
        /// </summary>
        public static LogForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogForm();
                return _instance;
            }
            private set { _instance = value; }
        }
        /// <summary>
        /// 设置输出
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        private void SetOutput(string text, Color color)
        {
            Action action = () =>
            {
                this.txtLog.Select(this.txtLog.Text.Length, 0);
                this.txtLog.Focus();
                if (color != null)
                {
                    txtLog.SelectionColor = color;
                }
                this.txtLog.AppendText(text);
                this.txtLog.AppendText(System.Environment.NewLine);
                //滚到最后
                this.txtLog.Select(txtLog.TextLength, 0);
                this.txtLog.Focus();
            };
            this.txtLog.Invoke(action);
        }
        /// <summary>
        /// 设置文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="color">颜色</param>
        /// <param name="withDateTime">带时间</param>
        public static void SetText(string text, Color color, bool withDateTime = true)
        {
            var @this = Instance;
            @this.Show();
            @this.SetOutput(withDateTime == true ? $"{DateTime.Now}:{text}" : text, color);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="text"></param>
        public static void Info(string text)
        {
            SetText(text, Color.Blue);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="text"></param>
        public static void Error(string text)
        {
            SetText(text, Color.Red);
        }
        /// <summary>
        /// Successful
        /// </summary>
        /// <param name="text"></param>
        public static void Successful(string text)
        {
            SetText(text, Color.DarkGreen);
        }
    }
}
