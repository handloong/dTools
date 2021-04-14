using dTools.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dTools.UI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        AppSetting appSetting = new AppSetting();
        private void FrmMain_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = appSetting;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            appSetting.SystemVersion += 1;
            propertyGrid1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.FixedSingle();
            appSetting = (AppSetting)propertyGrid1.SelectedObject;
            appSetting.SaveSelf(appSetting);
            LogForm.Info(appSetting.ToJson());

            var c = 0;
            var x5 = 0;

            for (int i = 1; i <= 10; i++)
            {
                c += i * i;
                x5 += i * 5;
                LogForm.Successful($"**:{c}  +%=:{x5}");
            }

            var res = RetryHelper.RetryOnAny<string>(times: 0, () =>
            {
                return SayHello();
            }, (i, ex) =>
            {
                MessageBox.Show($"出现异常_{i}_{ex.Message}");
            });
            MessageBox.Show(res);
        }

        private string SayHello()
        {
            //return "1";
            throw new Exception("1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 888; i++)
                {
                    richTextBox1.Write($"嘿嘿嘿嘿嘿{i}", Color.Red);
                }
            });
        }
    }
}
