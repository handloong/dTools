using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools.UI
{
    public class AppSetting
    {
        [CategoryAttribute("系统设置"), DescriptionAttribute("系统名称")]
        public string SystemName
        {
            get
            {
                return EasyINI.Read<string>("SystemName", "dTools.UI");
            }
            set
            {
                EasyINI.Write<string>("SystemName", value);
            }
        }

        [CategoryAttribute("系统设置"), DescriptionAttribute("系统版本")]
        public int SystemVersion
        {
            get
            {
                return EasyINI.Read<int>("SystemVersion", 1);
            }
            set
            {
                EasyINI.Write<int>("SystemVersion", value);
            }
        }
        [CategoryAttribute("用户信息"), DescriptionAttribute("用户姓名"), ReadOnlyAttribute(true)]
        public string Name { get; set; } = "邓振振";
        [CategoryAttribute("用户信息"), DescriptionAttribute("是否VIP"), ReadOnlyAttribute(true)]
        public bool Vip { get; set; }

        public void SaveSelf(AppSetting appSetting)
        {
            SystemName = appSetting.SystemName;
            SystemVersion = appSetting.SystemVersion;
            Name = appSetting.Name;
            Vip = appSetting.Vip;
        }
    }
}
